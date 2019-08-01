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
    /// Interaction logic for ScanItem.xaml
    /// </summary>
    public partial class ScanItem : Window
    {
        private bool _isDirty = false;
        
        public ScanItem()
        {
            InitializeComponent();
        }

        private void txtBox_ScanBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.ScanBarcode", conn))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Item_Barcode", txtBox_ScanBarcode.Text);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader read = cmd.ExecuteReader())
                        {
                            read.Read();

                            txtBox_Code.Text = (read["Item_Code"].ToString());
                            txtBox_Desc.Text = (read["Item_Desc"].ToString());
                            txtBox_Type.Text = (read["Item_Type"].ToString());
                            txtBox_Packing.Text = (read["Packing_UOM"].ToString());
                            txtBox_Volume.Text = (read["Volume_UOM"].ToString());
                            txtBox_Location.Text = (read["Default_Loc"].ToString());
                        }

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
            _isDirty = true;

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

        private void btn_Clear_Click(object sender, RoutedEventArgs e) //Clear TextBox
        {
            txtBox_ScanBarcode.Clear();
            txtBox_Code.Clear();
            txtBox_Desc.Clear();
            txtBox_Type.Clear();
            txtBox_Packing.Clear();
            txtBox_Volume.Clear();
            txtBox_Location.Clear();
        }

        private void btn_Reprint_Click(object sender, RoutedEventArgs e)
        {
            var ReprintBarcode = new ReprintBarcode();
            ReprintBarcode.Show();
        }


    }
}
