using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Configuration;

namespace Item
{
    /// <summary>
    /// Interaction logic for EditItemMaster.xaml
    /// </summary>
    
    public partial class EditItemMaster : Window
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

                        EditItemMaster eM = new EditItemMaster();

                        try
                        {
                            conn.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                for (int i = 0; i < eM.CodeQuantity(); i++)
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
        
        public EditItemMaster()
        {
            InitializeComponent();
        }

        private int CodeQuantity()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemMaster", conn);
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
        }

        private void btn_EditSearch_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Item_Desc, Item_Status, Item_Type, Packing_UOM, Volume_UOM, Default_Loc FROM ItemMaster WHERE Item_Code = '" + txtBox_EditCode.Text + "'";

                    try
                    {
                        conn.Open();

                        using (SqlDataReader read = cmd.ExecuteReader())
                        {
                            read.Read();

                            txtBox_EditDesc.Text = (read["Item_Desc"].ToString());
                            if (read["Item_Status"].ToString() == "0")
                            {
                                radioBtn_EditInactive.IsChecked = true;
                            }

                            else
                            {
                                radioBtn_EditActive.IsChecked = true;
                            }

                            comboBox_EditType.Text = (read["Item_Type"].ToString());
                            txtBox_EditPacking.Text = (read["Packing_UOM"].ToString());
                            txtBox_EditVolume.Text = (read["Volume_UOM"].ToString());
                            txtBox_EditLocation.Text = (read["Default_Loc"].ToString());
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
            }
        }

        private void btn_EditClear_Click(object sender, RoutedEventArgs e)
        {
            txtBox_EditCode.Clear();
            txtBox_EditDesc.Clear();
            txtBox_EditPacking.Clear();
            txtBox_EditVolume.Clear();
            txtBox_EditLocation.Clear();
            comboBox_EditType.SelectedIndex = -1;

            if (radioBtn_EditActive.IsChecked == true || radioBtn_EditInactive.IsChecked == true)
            {
                radioBtn_EditActive.IsChecked = false;
                radioBtn_EditInactive.IsChecked = false;
            }  
        }

        private void btn_EditSave_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                string creator = TrackName.name;

                using (SqlCommand Itemcmd = new SqlCommand())
                {
                    Itemcmd.Connection = conn;
                    Itemcmd.CommandType = CommandType.Text;
                    Itemcmd.CommandText = " UPDATE ItemMaster SET Item_Desc=@Item_Desc, Item_Status=@Item_Status, Item_Type=@Item_Type, Packing_UOM=@Packing_UOM, Volume_UOM=@Volume_UOM, Default_Loc=@Default_Loc, LastUpdate_By=@LastUpdate_By, LastUpdate_Dt=@LastUpdate_Dt WHERE Item_Code = '" + txtBox_EditCode.Text + "'";

                    try
                    {
                        if (radioBtn_EditActive.IsChecked == true)
                        {
                            Itemcmd.Parameters.AddWithValue("@Item_Desc", txtBox_EditDesc.Text);
                            Itemcmd.Parameters.AddWithValue("@Item_Status", 1);
                            Itemcmd.Parameters.AddWithValue("@Item_Type", ((ComboBoxItem)comboBox_EditType.SelectedItem).Content);
                            Itemcmd.Parameters.AddWithValue("@Packing_UOM", txtBox_EditPacking.Text);
                            Itemcmd.Parameters.AddWithValue("@Volume_UOM", txtBox_EditVolume.Text);
                            Itemcmd.Parameters.AddWithValue("@Default_Loc", txtBox_EditLocation.Text);
                            Itemcmd.Parameters.AddWithValue("@LastUpdate_By", creator);
                            Itemcmd.Parameters.AddWithValue("@LastUpdate_Dt", DateTime.Now.ToString());

                            try
                            {
                                conn.Open();
                                Itemcmd.ExecuteNonQuery();
                                System.Windows.Forms.MessageBox.Show("Done");
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("Fail (Active)");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                        else
                        {
                            Itemcmd.Parameters.AddWithValue("@Item_Desc", txtBox_EditDesc.Text);
                            Itemcmd.Parameters.AddWithValue("@Item_Status", 0);
                            Itemcmd.Parameters.AddWithValue("@Item_Type", ((ComboBoxItem)comboBox_EditType.SelectedItem).Content);
                            Itemcmd.Parameters.AddWithValue("@Packing_UOM", txtBox_EditPacking.Text);
                            Itemcmd.Parameters.AddWithValue("@Volume_UOM", txtBox_EditVolume.Text);
                            Itemcmd.Parameters.AddWithValue("@Default_Loc", txtBox_EditLocation.Text);
                            Itemcmd.Parameters.AddWithValue("@LastUpdate_By", creator);
                            Itemcmd.Parameters.AddWithValue("@LastUpdate_Dt", DateTime.Now.ToString());


                            try
                            {
                                conn.Open();
                                Itemcmd.ExecuteNonQuery();
                                System.Windows.Forms.MessageBox.Show("Done");
                            }

                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("Fail (Inactive)");
                            }
                            finally
                            {
                                conn.Close();
                            }  
                        }

                        //txtBox_EditCode.Clear();
                        //txtBox_EditDesc.Clear();
                        //txtBox_EditPacking.Clear();
                        //txtBox_EditVolume.Clear();
                        //txtBox_EditLocation.Clear();
                        //comboBox_EditType.SelectedIndex = -1;

                        /*if (radioBtn_EditActive.IsChecked == true || radioBtn_EditInactive.IsChecked == true)
                        {
                            radioBtn_EditActive.IsChecked = false;
                            radioBtn_EditInactive.IsChecked = false;
                        }*/

                        txtBox_EditDesc.IsReadOnly = true;
                        txtBox_EditLocation.IsReadOnly = true;
                        txtBox_EditPacking.IsReadOnly = true;
                        txtBox_EditVolume.IsReadOnly = true;
                        comboBox_EditType.IsHitTestVisible = false;
                        radioBtn_EditActive.IsEnabled = false;
                        radioBtn_EditInactive.IsEnabled = false;
                    }

                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Failed");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void btn_AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            var ItemMaster = new ItemMaster();
            ItemMaster.Show();
        }

        private void btn_EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_EditCode.Text != "")
            {
                if (System.Windows.MessageBox.Show("Do you want to edit this item?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    txtBox_EditDesc.IsReadOnly = false;
                    txtBox_EditLocation.IsReadOnly = false;
                    txtBox_EditPacking.IsReadOnly = false;
                    txtBox_EditVolume.IsReadOnly = false;
                    comboBox_EditType.IsHitTestVisible = true;
                    radioBtn_EditActive.IsEnabled = true;
                    radioBtn_EditInactive.IsEnabled = true;
                }
                else
                {
                    //Do Nothing
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Enter code number to select valid item.");
            }
        }

        private void btn_DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var DeleteItemMaster = new DeleteItemMaster();
            DeleteItemMaster.Show();
        }

        private void txtBox_EditCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedString = txtBox_EditCode.Text;

            List<string> autoList = new List<string>();
            autoList.Clear();

            var data = Model.GetData();
            
            foreach (string item in data)
            {
                if (!string.IsNullOrEmpty(txtBox_EditCode.Text))
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
            else if (txtBox_EditCode.Text.Equals(""))
            {
                lblSuggestion.Visibility = Visibility.Collapsed;
                lblSuggestion.ItemsSource = null;
            }
            else
            {
                lblSuggestion.Visibility = Visibility.Collapsed;
                lblSuggestion.ItemsSource = null;
            }
        }

        private void txtBox_EditCode_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
                    txtBox_EditCode.Text = lblSuggestion.SelectedItem.ToString();
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