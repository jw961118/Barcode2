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
using System.Text.RegularExpressions;

namespace Item
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
        
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        
        public void Reset()
        {
            txtBox_FirstName.Clear();
            txtBox_LastName.Clear();
            textBoxEmail.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }
        
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_FirstName.Text.Length == 0)
            {
                errormessage.Text = "Enter your first name.";
                textBoxEmail.Focus();
            }

            else if (txtBox_LastName.Text.Length == 0)
            {
                errormessage.Text = "Enter your last name.";
                textBoxEmail.Focus();
            }

            else if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                if (passwordBox1.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    passwordBox1.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox1.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {
                    errormessage.Text = "";
                    
                    SqlConnection con = new SqlConnection("Data Source=DESKTOP-Q1K44I8\\SA;Initial Catalog=Item2;Persist Security Info=True;User ID=sa;Password=softmap");
                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.uspAddUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("pFirstName", txtBox_FirstName.Text);
                    cmd.Parameters.AddWithValue("pLastName", txtBox_LastName.Text);
                    cmd.Parameters.AddWithValue("pLogin", email);
                    cmd.Parameters.AddWithValue("pPassword", password);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    errormessage.Text = "You have Registered successfully.";
                    Reset();
                }
            }
        }
    }
}
