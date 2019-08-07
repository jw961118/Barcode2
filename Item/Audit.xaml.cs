using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace Item
{
    /// <summary>
    /// Interaction logic for Audit.xaml
    /// </summary>
    public partial class Audit : Window
    {
        string caseSwitch;

        private bool IsItemCodeExist()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemMaster_TR WHERE Item_Code='" + txtBox_AuditCode.Text + "'", conn);
                int CodeExist = (int)cmd.ExecuteScalar();

                if (CodeExist > 0)
                {
                    return true;
                }

                else
                {
                    MessageBox.Show("Item Code Not Exist.");
                    dgv_Audit.ItemsSource = null;
                    txtBox_AuditCode.Clear();
                    return false;
                }

                //conn.Close();
            }
        }
        
        public Audit()
        {
            InitializeComponent();
        }

        private int AuditQuantity()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM AuditTrail", conn);
                int AuditQuantity = (int)cmd.ExecuteScalar();

                if (AuditQuantity > 0)
                {
                    return AuditQuantity;
                }

                else
                {
                    return 0;
                }
            }
        }

        private void getBinding()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT ItemNo, AuditAction, AuditDate, Item_Desc, Created_By, Created_Dt, LastUpdate_By , LastUpdate_Dt from ItemMaster_TR where Item_Code = '" + txtBox_AuditCode.Text + "'";

                    try
                    {
                        conn.Open();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_Audit.ItemsSource = dt.DefaultView;
                    }
                    catch
                    {
                        //MessageBox.Show("Search Failed");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void btn_Show_Click(object sender, RoutedEventArgs e)
        {
            if (IsItemCodeExist() == true)
            {
                getBinding();
            }
        }

        private void btn_ViewHistory_Click(object sender, RoutedEventArgs e)
        {
            if (IsItemCodeExist())
            {
                object item = dgv_Audit.SelectedItem;

                if (item != null)
                {
                    string ItemNo = (dgv_Audit.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    string date = (dgv_Audit.SelectedCells[7].Column.GetCellContent(item) as TextBlock).Text;

                    using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                    {
                        History eM = new History();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT distinct Item_Code, Item_Desc, Item_Status, Item_Type, Packing_UOM, Volume_UOM, Default_Loc, LastUpdate_Dt from ItemMaster_TR WHERE ItemNo = '" + ItemNo + "'";
                            
                            try
                            {
                                conn.Open();

                                using (SqlDataReader read = cmd.ExecuteReader())
                                {
                                    read.Read();

                                    eM.noti.Text = date;
                                    eM.txtBox_EditCode.Text = (read["Item_Code"].ToString());
                                    eM.txtBox_EditDesc.Text = (read["Item_Desc"].ToString());

                                    if (read["Item_Status"].ToString() == "0")
                                    {
                                        eM.radioBtn_EditInactive.IsChecked = true;
                                    }

                                    else
                                    {
                                        eM.radioBtn_EditActive.IsChecked = true;
                                    }

                                    eM.comboBox_EditType.Text = (read["Item_Type"].ToString());
                                    eM.txtBox_EditPacking.Text = (read["Packing_UOM"].ToString());
                                    eM.txtBox_EditVolume.Text = (read["Volume_UOM"].ToString());
                                    eM.txtBox_EditLocation.Text = (read["Default_Loc"].ToString());
                                }
                            }
                            catch
                            {
                                System.Windows.MessageBox.Show("Search Failed. Item not exist.");
                            }
                            finally
                            {
                                conn.Close();
                            }
                            //eM.Show();
                        }

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT FieldName, OldValue FROM AuditTrail inner join ItemMaster_TR on convert(nvarchar(50),UpdateDate,20) = convert(nvarchar(50),AuditDate,20) WHERE PK = '<ItemNo=" + ItemNo + ">' AND LastUpdate_Dt =convert(nvarchar(50),'" + date + "',20) AND OldValue IS NOT NULL";

                            try
                            {
                                conn.Open();

                                using (SqlDataReader read = cmd.ExecuteReader())
                                {
                                    while (read.Read())
                                    {
                                        caseSwitch = (read["FieldName"].ToString());

                                        switch (caseSwitch)
                                        {
                                            case "Item_Desc":
                                                eM.txtBox_EditDesc.Text = (read["OldValue"].ToString());
                                                break;
                                            case "Item_Status":
                                                if (read["OldValue"].ToString() == "0")
                                                {
                                                    eM.radioBtn_EditInactive.IsChecked = true;
                                                    break;
                                                }

                                                else
                                                {
                                                    eM.radioBtn_EditActive.IsChecked = true;
                                                    break;
                                                }
                                            case "Item_Type":
                                                eM.comboBox_EditType.Text = (read["OldValue"].ToString());
                                                break;
                                            case "Packing_UOM":
                                                eM.txtBox_EditPacking.Text = (read["OldValue"].ToString());
                                                break;
                                            case "Volume_UOM":
                                                eM.txtBox_EditVolume.Text = (read["OldValue"].ToString());
                                                break;
                                            case "Default_Loc":
                                                eM.txtBox_EditLocation.Text = (read["OldValue"].ToString());
                                                break;
                                        }
                                    }     
                                }
                            }
                            catch
                            {
                                System.Windows.MessageBox.Show("Search Failed. Item not exist.");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                        eM.Show();
                    }
                }
                else
                {
 
                }

            }
            }
    }
}
