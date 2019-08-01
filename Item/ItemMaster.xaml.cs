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
    /// Interaction logic for ItemMaster.xaml
    /// </summary>
    public partial class ItemMaster : Window
    {
        private bool _isDirty = false;

        private bool IsInputValid()
        {
            if (txtBox_Code.Text == "" || txtBox_Desc.Text == "" || txtBox_Packing.Text == "" || txtBox_Volume.Text == "" || txtBox_Location.Text == "" || (radioBtn_Active.IsChecked == false && radioBtn_Inactive.IsChecked == false))
            {
                MessageBox.Show("Please fill up all details.");
                return false;
            }

            else
            {
                return true;
            }
        }

        private bool IsItemCodeValid()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemMaster WHERE Item_Code='" + txtBox_Code.Text + "'", conn);
                int ItemCodeExist = (int)cmd.ExecuteScalar();

                if (ItemCodeExist > 0)
                {
                    MessageBox.Show("Item Code Exist.");
                    return false;
                }

                else
                {
                    return true;
                }

                //conn.Close();
            }
        }

        public ItemMaster()
        {
            InitializeComponent();
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBox_Code.Clear();
            txtBox_Desc.Clear();
            txtBox_Packing.Clear();
            txtBox_Volume.Clear();
            txtBox_Location.Clear();
            comboBox_Type.SelectedIndex = -1;

            if (radioBtn_Active.IsChecked == true || radioBtn_Inactive.IsChecked == true)
            {
                radioBtn_Active.IsChecked = false;
                radioBtn_Inactive.IsChecked = false;
            }            
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (IsInputValid())
            {
                if (IsItemCodeValid())
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                    {
                        string creator = TrackName.name;

                        if (radioBtn_Active.IsChecked == true)
                        {
                            using (SqlCommand Itemcmd = new SqlCommand("dbo.InsertItem", conn))
                            {
                                Itemcmd.CommandType = CommandType.StoredProcedure;

                                Itemcmd.Parameters.AddWithValue("@Item_Code", txtBox_Code.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Desc", txtBox_Desc.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Status", 1);
                                Itemcmd.Parameters.AddWithValue("@Item_Type", ((ComboBoxItem)comboBox_Type.SelectedItem).Content);
                                Itemcmd.Parameters.AddWithValue("@Packing_UOM", txtBox_Packing.Text);
                                Itemcmd.Parameters.AddWithValue("@Volume_UOM", txtBox_Volume.Text);
                                Itemcmd.Parameters.AddWithValue("@Default_Loc", txtBox_Location.Text);
                                Itemcmd.Parameters.AddWithValue("@Created_Dt", DateTime.Now.ToString());
                                Itemcmd.Parameters.AddWithValue("@Created_By", creator);

                                try
                                {
                                    conn.Open();
                                    Itemcmd.ExecuteNonQuery();
                                    MessageBox.Show("Done");
                                }

                                catch
                                {
                                    MessageBox.Show("Fail (Active)");
                                }
                                finally
                                {
                                    conn.Close();
                                }
                            }
                        }

                        else
                        {
                            using (SqlCommand Itemcmd = new SqlCommand("dbo.InsertItem", conn))
                            {
                                Itemcmd.CommandType = CommandType.StoredProcedure;

                                Itemcmd.Parameters.AddWithValue("@Item_Code", txtBox_Code.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Desc", txtBox_Desc.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Status", 0);
                                Itemcmd.Parameters.AddWithValue("@Item_Type", ((ComboBoxItem)comboBox_Type.SelectedItem).Content);
                                Itemcmd.Parameters.AddWithValue("@Packing_UOM", txtBox_Packing.Text);
                                Itemcmd.Parameters.AddWithValue("@Volume_UOM", txtBox_Volume.Text);
                                Itemcmd.Parameters.AddWithValue("@Default_Loc", txtBox_Location.Text);
                                Itemcmd.Parameters.AddWithValue("@Created_Dt", DateTime.Now.ToString());
                                Itemcmd.Parameters.AddWithValue("@Created_By", creator);

                                try
                                {
                                    conn.Open();
                                    Itemcmd.ExecuteNonQuery();
                                    MessageBox.Show("Done");
                                }

                                catch
                                {
                                    MessageBox.Show("Fail (Inactive)");
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
        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            if (_isDirty)
            {
                if (MessageBox.Show("Do you want to close this window?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.Close(); // Close the window  
                }
                else
                {
                    // Do not close the window  
                }
            }
            else
            {
                this.Close();
            }
        }

        private void txtBox_Code_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }

        private void txtBox_Desc_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }

        private void radioBtn_Active_Checked(object sender, RoutedEventArgs e)
        {
            _isDirty = true;
        }

        private void radioBtn_Inactive_Checked(object sender, RoutedEventArgs e)
        {
            _isDirty = true;
        }

        private void comboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isDirty = true;
        }

        private void txtBox_Packing_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }

        private void txtBox_Volume_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }

        private void txtBox_Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }
    }
}
