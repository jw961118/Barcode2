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
using System.Drawing;
using System.Drawing.Imaging;
using OnBarcode.Barcode;

namespace Item
{
    /// <summary>
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        private bool _isDirty = false;
        
        private void DrawBarcode()
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True");
            using (SqlCommand Barcodecmd = new SqlCommand())
            {
                Barcodecmd.Connection = conn;
                Barcodecmd.CommandType = CommandType.Text;
                Barcodecmd.CommandText = "select Item_Barcode from ItemBarcode order by Barcode_ID DESC";

                try
                {
                    conn.Open();

                    using (SqlDataReader read = Barcodecmd.ExecuteReader())
                    {
                        read.Read();
                        
                        // Create linear barcode object
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
                    //MessageBox.Show("Create New Barcode Done");
                }
                catch
                {
                    MessageBox.Show("Create New Barcode Failed");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private bool IsAddItemValid()
        {
            if (txtBox_BatchNo.Text == "" || txtBox_BatchSize.Text == "" || comboBox_ItemName.Text == "")
            {
                MessageBox.Show("Please fill up all details.");
                return false;
            }

            else
            {
                return true;
            }
        }

        private bool IsBatchNoValid()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ItemBarcode WHERE BatchNumber='" + txtBox_BatchNo.Text + "'", conn);
                int BatchNoExist = (int) cmd.ExecuteScalar();

                if (BatchNoExist > 0)
                {
                    MessageBox.Show("Batch Number Exist.");
                    return false;
                }

                else
                {
                    return true;
                }

                //conn.Close();
            }
        }

        public AddItem()
        {
            InitializeComponent();
            BindItemName(comboBox_ItemName);
        }

        public void BindItemName(ComboBox comboBox_ItemName)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True");
            SqlDataAdapter da = new SqlDataAdapter("Select Item_Code, Item_Desc FROM ItemMaster WHERE Item_Status = 1", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "ItemMaster");
            comboBox_ItemName.ItemsSource = ds.Tables[0].DefaultView;
            comboBox_ItemName.DisplayMemberPath = ds.Tables[0].Columns["Item_Desc"].ToString();
            comboBox_ItemName.SelectedValuePath = ds.Tables[0].Columns["Item_Code"].ToString();
        }

        /*public void BindBatchSize(TextBox txtBox_BatchSize)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item;Integrated Security=True");
            using (SqlCommand cmd = new SqlCommand("select ItemBatchSize from tbl_ItemMaster where ItemCode = @ItemCode", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@ItemCode", comboBox_ItemName.SelectedValue.ToString()));
                try
                {
                    conn.Open();

                    using (SqlDataReader read = cmd.ExecuteReader())
                    {
                        read.Read();
                        txtBox_BatchSize.Text = (read["ItemBatchSize"].ToString());
                    }

                }
                catch
                {
                    MessageBox.Show("Unable to bind batch size");
                }
                finally
                {
                    conn.Close();
                }
            }
        }*/

        private void comboBox_ItemName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //BindBatchSize(txtBox_BatchSize);
            _isDirty = true;
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
           if (IsAddItemValid())
            {
                if (IsBatchNoValid())
                {
                    int quantity = int.Parse(txtBox_BatchSize.Text);

                    try
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Integrated Security=True");
                            using (SqlCommand cmd = new SqlCommand("insert into ItemBarcode (Item_Code, BatchNumber) values (@Item_Code, @BatchNumber)", conn))
                            {
                                cmd.Parameters.Add(new SqlParameter("@Item_Code", comboBox_ItemName.SelectedValue.ToString()));
                                cmd.Parameters.Add(new SqlParameter("@BatchNumber", txtBox_BatchNo.Text));

                                try
                                {
                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    DrawBarcode();
                                }
                                catch
                                {
                                    MessageBox.Show("Insert Failed");
                                }
                                finally
                                {
                                    conn.Close();
                                }
                            }
                        }
                        MessageBox.Show("Create " + quantity + " Barcodes DONE");
                    }

                    catch
                    {
                        MessageBox.Show("Create New Barcodes FAILED");
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

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBox_BatchNo.Clear();
            comboBox_ItemName.SelectedIndex = 0;
            txtBox_BatchSize.Clear();
        }

        private void txtBox_BatchNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }

        private void txtBox_BatchSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isDirty = true;
        }
    }
}