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
    /// Interaction logic for DeleteItemMaster.xaml
    /// </summary>
    public partial class DeleteItemMaster : Window
    {
        private bool IsInputValid()
        {
            if (txtBox_DeleteCode.Text == "")
            {
                MessageBox.Show("Please fill up Item Code.");
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public DeleteItemMaster()
        {
            InitializeComponent();
        }

        private void btn_DeleteSearch_Click(object sender, RoutedEventArgs e)
        {
            if (IsInputValid())
            {
                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT Item_Desc FROM ItemMaster WHERE Item_Code = '" + txtBox_DeleteCode.Text + "'";

                        try
                        {
                            conn.Open();

                            using (SqlDataReader read = cmd.ExecuteReader())
                            {
                                read.Read();

                                txtBox_DeleteDesc.Text = (read["Item_Desc"].ToString());
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Item Not Exist");
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        private void btn_DeleteClear_Click(object sender, RoutedEventArgs e)
        {
            txtBox_DeleteCode.Clear();
            txtBox_DeleteDesc.Clear();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (IsInputValid())
            {
                if (MessageBox.Show("Do you want to delete this item?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "DELETE FROM ItemMaster WHERE Item_Code = '" + txtBox_DeleteCode.Text + "'";

                            try
                            {
                                conn.Open();

                                cmd.ExecuteNonQuery();
                                //MessageBox.Show("Done");
                                txtBox_DeleteCode.Clear();
                                txtBox_DeleteDesc.Clear();
                            }
                            catch
                            {
                                MessageBox.Show("Unable to delete Item");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                else
                {
                    //Do Nothing
                }
            }
        }
    }
}