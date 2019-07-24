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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            //if (txtBox_Email.Text.Length != 0)
            if (txtBox_Email.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                txtBox_Email.Focus();
            }
            
            else
            {
                string email = txtBox_Email.Text;
                string password = passwordBox.Password;

                //string email = "jw_law@hotmail.com";
                //string password = "12345678";

                SqlConnection con = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;User ID=sa;Password=softmap");
                con.Open();
                
                SqlCommand cmd = new SqlCommand("Select * from Registration where Email='" + email + "'  and Password='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    //string username = dataSet.Tables[0].Rows[0]["FirstName"].ToString() + " " + dataSet.Tables[0].Rows[0]["LastName"].ToString();
                    //welcome.TextBlockName.Text = username;//Sending value from one form to another form.  
                    var MainWindow = new MainWindow();
                    MainWindow.Show();
                    Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing EmailID/Password.";
                }
                
                con.Close();
            }
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            var Registration = new Registration();
            Registration.Show();
            Close();
        }
    }
}
