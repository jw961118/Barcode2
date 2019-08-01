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
        string barcodenumber;

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

        private bool IsBatchNumberExist()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemBarcode WHERE BatchNumber='" + txtBox_PrintBarcode.Text + "'", conn);
                int BatchNumberExist = (int)cmd.ExecuteScalar();

                if (BatchNumberExist > 0)
                {
                    return true;
                }

                else
                {
                    MessageBox.Show("Batch Number Not Exist.");
                    return false;
                }

                //conn.Close();
            }
        }

        private int BarcodeQuantity()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemBarcode WHERE BatchNumber='" + txtBox_PrintBarcode.Text + "'", conn);
                int BatchNumberExist = (int)cmd.ExecuteScalar();

                if (BatchNumberExist > 0)
                {
                    return BatchNumberExist;
                }

                else
                {
                    return 0;
                }

                //conn.Close();
            }
        }

        private int BatchNumberQuantity()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) BatchNumber FROM ItemBarcode", conn);
                int BatchNumberQuantity = (int)cmd.ExecuteScalar();

                if (BatchNumberQuantity > 0)
                {
                    return BatchNumberQuantity;
                }

                else
                {
                    return 0;
                }
            }
        }

        class Model
        {
            static public List<string> GetData()
            {
                List<string> data = new List<string>();

                using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT distinct BatchNumber FROM ItemBarcode";

                        ReprintBarcode rb = new ReprintBarcode();

                        try
                        {
                            conn.Open();

                            using (SqlDataReader read = cmd.ExecuteReader())
                            {
                                for (int i = 0; i < rb.BatchNumberQuantity(); i++)
                                {
                                    read.Read();
                                    data.Add(read["BatchNumber"].ToString());
                                }
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

                    return data;
                }
            }
        }

        private void txtBox_PrintBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedString = txtBox_PrintBarcode.Text;

            List<string> autoList = new List<string>();
            autoList.Clear();

            var data = Model.GetData();

            foreach (string item in data)
            {
                if (!string.IsNullOrEmpty(txtBox_PrintBarcode.Text))
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
            else if (txtBox_PrintBarcode.Text.Equals(""))
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

        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            if (IsBarcodeValid())
            {
                if (IsBatchNumberExist())
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Persist Security Info=True;User ID=sa;Password=softmap"))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = " SELECT Item_Barcode FROM ItemBarcode WHERE BatchNumber = '" + txtBox_PrintBarcode.Text + "'";
                            try
                            {
                                conn.Open();
                                using (SqlDataReader read = cmd.ExecuteReader())
                                {
                                    for (int i = 0; i < BarcodeQuantity(); i++)
                                    {
                                        read.Read();
                                        barcodenumber = (read["Item_Barcode"].ToString());
                                        Linear barcode = new Linear();
                                        // Set barcode symbology type to Code-128
                                        barcode.Type = BarcodeType.CODE128;
                                        // Set barcode data to encode
                                        barcode.Data = barcodenumber.ToString();
                                        // Set barcode bar width (X dimension) in pixel
                                        barcode.X = 1;
                                        // Set barcode bar height (Y dimension) in pixel
                                        barcode.Y = 60;
                                        // Draw & print generated barcode to png image file
                                        barcode.drawBarcode("C:/Users/Jwen/Desktop/BarcodeImage/Item_" + barcode.Data + ".jpg");
                                    }
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
                    txtBox_PrintBarcode.Text = lblSuggestion.SelectedItem.ToString();
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
