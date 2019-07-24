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
    /// Interaction logic for EditItemMaster.xaml
    /// </summary>
    public partial class EditItemMaster : Window
    {
        public EditItemMaster()
        {
            InitializeComponent();
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
                        MessageBox.Show("Search Failed");
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
                using (SqlCommand Itemcmd = new SqlCommand())
                {
                    Itemcmd.Connection = conn;
                    Itemcmd.CommandType = CommandType.Text;
                    Itemcmd.CommandText = " UPDATE ItemMaster SET Item_Desc=@Item_Desc, Item_Status=@Item_Status, Item_Type=@Item_Type, Packing_UOM=@Packing_UOM, Volume_UOM=@Volume_UOM, Default_Loc=@Default_Loc WHERE Item_Code = '" + txtBox_EditCode.Text + "'";

                    try
                    {
                        if (radioBtn_EditActive.IsChecked == true)
                        {
                                //Itemcmd.Parameters.AddWithValue("@Item_Code", txtBox_EditCode.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Desc", txtBox_EditDesc.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Status", 1);
                                Itemcmd.Parameters.AddWithValue("@Item_Type", ((ComboBoxItem)comboBox_EditType.SelectedItem).Content);
                                Itemcmd.Parameters.AddWithValue("@Packing_UOM", txtBox_EditPacking.Text);
                                Itemcmd.Parameters.AddWithValue("@Volume_UOM", txtBox_EditVolume.Text);
                                Itemcmd.Parameters.AddWithValue("@Default_Loc", txtBox_EditLocation.Text);
                                //Itemcmd.Parameters.AddWithValue("@Created_Dt", DateTime.Now.ToString());

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

                        else
                        {
                                //Itemcmd.Parameters.AddWithValue("@Item_Code", txtBox_EditCode.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Desc", txtBox_EditDesc.Text);
                                Itemcmd.Parameters.AddWithValue("@Item_Status", 0);
                                Itemcmd.Parameters.AddWithValue("@Item_Type", ((ComboBoxItem)comboBox_EditType.SelectedItem).Content);
                                Itemcmd.Parameters.AddWithValue("@Packing_UOM", txtBox_EditPacking.Text);
                                Itemcmd.Parameters.AddWithValue("@Volume_UOM", txtBox_EditVolume.Text);
                                Itemcmd.Parameters.AddWithValue("@Default_Loc", txtBox_EditLocation.Text);
                                //Itemcmd.Parameters.AddWithValue("@Created_Dt", DateTime.Now.ToString());

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

                    catch
                    {
                        MessageBox.Show("Failed");
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
