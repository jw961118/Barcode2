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
        //string caseSwitch;

        private bool IsItemCodeExist()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemMaster WHERE Item_Code='" + txtBox_AuditCode.Text + "'", conn);
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

        private bool IsItemCodeChange()
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
                    MessageBox.Show("Item not modified.");
                    dgv_Audit.ItemsSource = null;
                    dgv_Result.ItemsSource = null;
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
                    cmd.CommandText = "SELECT row_number() OVER (ORDER BY AuditId) Iteration, ItemNo, AuditAction, AuditDate, LastUpdate_By , LastUpdate_Dt from ItemMaster_TR where Item_Code = '" + txtBox_AuditCode.Text + "'";

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
                        MessageBox.Show("Unable to show record. void getBinding()");
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
            //if (IsItemCodeExist() == true)
            //{
                if (IsItemCodeChange() == true)
                {
                    getBinding();
                    dgv_Result.ItemsSource = null;
                }
            //}
        }

        private void btn_ViewHistory_Click(object sender, RoutedEventArgs e)
        {

            if (IsItemCodeChange())
            {
                object item = dgv_Audit.SelectedItem;

                if (item != null)
                {
                    string ItemNo = (dgv_Audit.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                    string date = (dgv_Audit.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
                    string action = (dgv_Audit.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;

                    using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                    {
                        History eM = new History();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT Type, TableName, PK, FieldName, OldValue, NewValue, UpdateDate, UserName FROM AuditTrail inner join ItemMaster_TR on convert(nvarchar(50),UpdateDate,20) = convert(nvarchar(50),AuditDate,20) WHERE PK = '<ItemNo=" + ItemNo + ">' AND LastUpdate_Dt =convert(nvarchar(50),'" + date + "',20) AND OldValue IS NOT NULL";

                            try
                            {
                                conn.Open();

                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                dgv_Result.ItemsSource = dt.DefaultView;

                            }
                            catch
                            {
                                System.Windows.MessageBox.Show("Unable to show current item's details.");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }

                }
            }

        }

        private void dgv_Audit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_ViewHistory.IsEnabled = true;
            btn_ViewDetails.IsEnabled = true;
        }

        private void btn_ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            //if (IsItemCodeExist())
            //{
                if (IsItemCodeChange())
                {
                    object item = dgv_Audit.SelectedItem;

                    if (item != null)
                    {
                        string ItemNo = (dgv_Audit.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                        string date = (dgv_Audit.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
                        string action = (dgv_Audit.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text; 

                        //if (action == "I" || action == "D")
                        {
                            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                            {
                                History eM = new History();

                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    cmd.Connection = conn;
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT Item_Code, Item_Desc, Item_Status, Item_Type, Packing_UOM, Volume_UOM, Default_Loc, LastUpdate_Dt from ItemMaster_TR WHERE ItemNo = '" + ItemNo + "' AND LastUpdate_Dt =convert(nvarchar(50),'" + date + "',20)";

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
                                        System.Windows.MessageBox.Show("Unable to show current item's details.");
                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                }
                                eM.Show();
                            }
                        }
                    }
                    else
                    {

                    }
                }
            //}
        }

        private void txtBox_AuditCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_Show_Click(sender, e);
            }
        }
    }
}
