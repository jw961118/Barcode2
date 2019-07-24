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
using OnBarcode.Barcode;

namespace Item
{
    /// <summary>
    /// Interaction logic for ReprintBarcode.xaml
    /// </summary>
    public partial class ReprintBarcode : Window
    {
        public ReprintBarcode()
        {
            InitializeComponent();
        }

        private bool IsBarcodeValid()
        {
            if (txtBox_PrintBarcode.Text == "")
            {
                MessageBox.Show("Please fill up all details.");
                return false;
            }

            else
            {
                return true;
            }
        }

        private bool IsBarcodeExist()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemBarcode WHERE Item_Barcode='" + txtBox_PrintBarcode.Text + "'", conn);
                int BarcodeExist = (int)cmd.ExecuteScalar();

                if (BarcodeExist > 0)
                {
                    return true;
                }

                else
                {
                    MessageBox.Show("Barcode Not Exist.");
                    return false;
                }

                //conn.Close();
            }
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            if (IsBarcodeValid())
            {
                if (IsBarcodeExist())
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Persist Security Info=True;User ID=sa;Password=softmap"))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = " SELECT Item_Barcode FROM ItemBarcode WHERE Item_Barcode = '" + txtBox_PrintBarcode.Text + "'";

                            try
                            {
                                conn.Open();

                                using (SqlDataReader read = cmd.ExecuteReader())
                                {
                                    read.Read();
                                    Linear barcode = new Linear();
                                    // Set barcode symbology type to Code-128
                                    barcode.Type = BarcodeType.CODE128;
                                    // Set barcode data to encode
                                    barcode.Data = read["Item_Barcode"].ToString();
                                    // Set barcode bar width (X dimension) in pixel
                                    barcode.X = 1;
                                    // Set barcode bar height (Y dimension) in pixel
                                    barcode.Y = 60;
                                    // Draw & print generated barcode to png image file
                                    barcode.drawBarcode("C:/Users/Jwen/Desktop/BarcodeImage/Item_" + barcode.Data + ".jpg");
                                }
                                MessageBox.Show("Reprint Barcode Done");
                            }
                            catch
                            {
                                MessageBox.Show("Reprint Barcode Failed");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                        txtBox_PrintBarcode.Clear();
                    }
                }
            }
        }
    }
}
