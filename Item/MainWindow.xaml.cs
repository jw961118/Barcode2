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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Item
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_ItemMaster_Click(object sender, RoutedEventArgs e)
        {
            var ItemMaster = new ItemMaster();
            ItemMaster.Show();
        }

        private void btn_AddItem_Click(object sender, RoutedEventArgs e)
        {
            var AddItem = new AddItem();
            AddItem.Show();
        }

        private void btn_ScanItem_Click(object sender, RoutedEventArgs e)
        {
            var ScanItem = new ScanItem();
            ScanItem.Show();
        }

        private void btn_DeleteItemMaster_Click(object sender, RoutedEventArgs e)
        {
            var DeleteItemMaster = new DeleteItemMaster();
            DeleteItemMaster.Show();
        }

        private void btn_EditItemMaster_Click(object sender, RoutedEventArgs e)
        {
            var EditItemMaster = new EditItemMaster();
            EditItemMaster.Show();
        }

        private void btn_Reprint_Click(object sender, RoutedEventArgs e)
        {
            var ReprintBarcode = new ReprintBarcode();
            ReprintBarcode.Show();
        }
    }
}
