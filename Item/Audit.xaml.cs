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
        public Audit()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //getBinding();
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

        private void getUBinding()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Type, FieldName, OldValue, NewValue, UpdateDate, UserName from AuditTrail, ItemMaster WHERE Type = 'U' AND Item_Code = '" + txtBox_AuditCode.Text + "'";

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

        private void getIBinding()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Type, FieldName, OldValue, NewValue, UpdateDate, UserName from AuditTrail, ItemMaster WHERE Type = 'I' AND Item_Code = '" + txtBox_AuditCode.Text + "'";

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

        private void getDBinding()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Type, FieldName, OldValue, NewValue, UpdateDate, UserName from AuditTrail, ItemMaster WHERE Type = 'D' AND Item_Code = '" + txtBox_AuditCode.Text + "'";

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

        private void getABinding()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Type, FieldName, OldValue, NewValue, UpdateDate, UserName from AuditTrail, ItemMaster WHERE Item_Code = '" + txtBox_AuditCode.Text + "'";

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

        private void getBinding()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Type, FieldName, OldValue, NewValue, UpdateDate, UserName from AuditTrail";

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
            if (txtBox_AuditCode.Text == "")
            {
                getBinding();
            }

            else
            {
                if (rBtn_Update.IsChecked == true)
                {
                    getUBinding();
                }
                else if (rBtn_Insert.IsChecked == true)
                {
                    getIBinding();
                }
                else if (rBtn_Delete.IsChecked == true)
                {
                    getDBinding();
                }
                else if (rBtn_All.IsChecked == true)
                {
                    getABinding();
                }
            }
        }
    }
}
