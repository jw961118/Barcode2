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
        class Model
        {
            static public List<string> GetData()
            {
                List<string> stringCollection = new List<string>();

                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT distinct Item_Code from ItemMaster";

                        DeleteItemMaster dM = new DeleteItemMaster();

                        try
                        {
                            conn.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                for (int i = 0; i < dM.CodeQuantity(); i++)
                                {
                                    reader.Read();
                                    stringCollection.Add(reader["Item_Code"].ToString());
                                }
                            }
                        }
                        catch
                        {
                            //System.Windows.MessageBox.Show("Search Failed");
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }

                    return stringCollection;
                }
            }
        }

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

        private int CodeQuantity()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) Item_Code FROM ItemMaster", conn);
                int CodeExist = (int)cmd.ExecuteScalar();

                if (CodeExist > 0)
                {
                    return CodeExist;
                }

                else
                {
                    return 0;
                }

                //conn.Close();
            }
        } //Determine the total number of item code in Item Master

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
        } //Search the details of item based on Item code given

        private void btn_DeleteClear_Click(object sender, RoutedEventArgs e)
        {
            txtBox_DeleteCode.Clear();
            txtBox_DeleteDesc.Clear();
        } //Clear the details of item

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
        } //Delete the item from Item Master

        private void txtBox_DeleteCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedString = txtBox_DeleteCode.Text;

            List<string> autoList = new List<string>();
            autoList.Clear();

            var data = Model.GetData();
            
            foreach (string item in data)
            {
                if (!string.IsNullOrEmpty(txtBox_DeleteCode.Text))
                {
                    if (item.Contains(typedString))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                lblSuggestion.ItemsSource = autoList;
                lblSuggestion.Visibility = Visibility.Visible;
            }
            else if (txtBox_DeleteCode.Text.Equals(""))
            {
                lblSuggestion.Visibility = Visibility.Collapsed;
                lblSuggestion.ItemsSource = null;
            }
            else
            {
                lblSuggestion.Visibility = Visibility.Collapsed;
            }
                    
        } //Autocomplete Textbox happen HERE

        private void txtBox_DeleteCode_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            {
                if (e.Key == Key.Down)
                {
                    lblSuggestion.Focus();
                }

            }
        }

        private void lblSuggestion_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (ReferenceEquals(sender, lblSuggestion))
            {
                if (e.Key == Key.Enter)
                {
                    txtBox_DeleteCode.Text = lblSuggestion.SelectedItem.ToString();
                    lblSuggestion.Visibility = Visibility.Collapsed;
                }

                if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    lblSuggestion.Items.MoveCurrentToNext();
                }
                if (e.Key == Key.Up)
                {
                    e.Handled = true;
                    lblSuggestion.Items.MoveCurrentToPrevious();
                }
            }

        }

        private void lblSuggestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblSuggestion.ItemsSource != null)
            {
                lblSuggestion.KeyDown += lblSuggestion_KeyDown;
            }
        }
    }
}