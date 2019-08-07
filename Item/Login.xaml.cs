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

    public class TrackName
    {
        private static string nm;

        public static string name
        {
            get
            {
                return nm;
            }
            set
            {
                nm = value;
            }
        }
    }
    
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_Email.Text.Length != 0)
            //if (txtBox_Email.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                txtBox_Email.Focus();
            }
            
            else
            {
                //string email = txtBox_Email.Text;
                //string password = passwordBox.Password;

                string email = "jw@hotmail.com";
                string password = "8888";

                SqlConnection con = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;User ID=sa;Password=softmap");
                con.Open();
                
                SqlCommand cmd = new SqlCommand("dbo.uspLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pLoginName", email);
                cmd.Parameters.AddWithValue("@pPassword", password);
                cmd.Parameters.Add("@pStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                int retunvalue = (Convert.ToInt32(cmd.Parameters["@pStatus"].Value));
                
                if (retunvalue == 1)
                {
                    TrackName.name = email;

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
