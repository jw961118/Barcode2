--Set up the tables
if exists (select * from sysobjects where id = object_id(N'[dbo].[AuditTrail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AuditTrail]
go
create table AuditTrail (
	Type char(1), 
	TableName varchar(128), 
	PK varchar(1000), 
	FieldName varchar(128), 
	OldValue varchar(1000), 
	NewValue varchar(1000), 
	UpdateDate datetime, 
	UserName varchar(128)
)
go
