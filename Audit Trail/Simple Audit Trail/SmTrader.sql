Create trigger tr_SmTrader on SmTrader for insert, update, delete --Set Trigger Name
as

declare @bit int ,
	@field int ,
	@maxfield int ,
	@char int ,
	@fieldname varchar(128) ,
	@TableName varchar(128) ,
	@PKCols varchar(1000) ,
	@sql varchar(2000), 
	@UpdateDate varchar(21) ,
	@UserName varchar(128) ,
	@Type char(1) ,
	@UserID varchar(128),
	@PKSelect varchar(1000)
	
	select @TableName = 'SmTrader'--Set Table Name

	-- date and user
	select 	@UserName = host_name() ,
		@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

	-- Action
	if exists (select * from inserted)
		if exists (select * from deleted)
			select @Type = 'U'
		else
			select @Type = 'I'
	else
		select @Type = 'D'
	
	-- get list of columns
	select * into #ins from inserted
	select * into #del from deleted
	
	-- Get primary key columns for full outer join
	select	@PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
	from	INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
		INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
	where 	pk.TABLE_NAME = @TableName
	and	CONSTRAINT_TYPE = 'PRIMARY KEY'
	and	c.TABLE_NAME = pk.TABLE_NAME
	and	c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
	
	-- Get primary key select for insert
	select @PKSelect = coalesce(@PKSelect+'+','') + '''<' + COLUMN_NAME + '=''+convert(varchar(100),coalesce(i.' + COLUMN_NAME +',d.' + COLUMN_NAME + '))+''>''' 
	from	INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
		INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
	where 	pk.TABLE_NAME = @TableName
	and	CONSTRAINT_TYPE = 'PRIMARY KEY'
	and	c.TABLE_NAME = pk.TABLE_NAME
	and	c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
	
	if @PKCols is null
	begin
		raiserror('no PK on table %s', 16, -1, @TableName)
		return
	end
	
	select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
	while @field < @maxfield
	begin
		select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
		select @bit = (@field - 1 )% 8 + 1
		select @bit = power(2,@bit - 1)
		select @char = ((@field - 1) / 8) + 1
		select @UserID = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName AND COLUMN_NAME = 'User_ID'
		
		if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
		begin
			select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
			select @sql = 		'insert AuditTrail (Type, TableName, PK, FieldName, OldValue, NewValue, UpdateDate, UserName)'
			select @sql = @sql + 	' select ''' + @Type + ''''
			select @sql = @sql + 	',''' + @TableName + ''''
			select @sql = @sql + 	',' + @PKSelect
			select @sql = @sql + 	',''' + @fieldname + ''''
			select @sql = @sql + 	',convert(varchar(1000),d.' + @fieldname + ')'
			select @sql = @sql + 	',convert(varchar(1000),i.' + @fieldname + ')'
			select @sql = @sql + 	',''' + @UpdateDate + ''''
			select @sql = @sql + 	',convert(varchar(1000),i.' + @UserID + ')'
			select @sql = @sql + 	' from #ins i full outer join #del d'
			select @sql = @sql + 	@PKCols
			select @sql = @sql + 	' where i.' + @fieldname + ' <> d.' + @fieldname 
			select @sql = @sql + 	' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
			select @sql = @sql + 	' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
			exec (@sql)
		end
	end
go
