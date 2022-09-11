using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;

namespace zxc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int forFirstUserTextBoxSelect = 0;
        int forFirstPassTextBoxSelect = 0;
        int k = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UserTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text == null || (forFirstUserTextBoxSelect == 0 || UserTextBox.Text == "Username"))
            {
                UserTextBox.Text = "";
                UserTextBox.Opacity = 1;
            }
            forFirstUserTextBoxSelect = 1;
        }

        private void UserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text == "")
            {
                UserTextBox.Text = "Username";
                UserTextBox.Opacity = 0.5;
            }
        }
        private void PassTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PassTextBox.Password == null || (forFirstPassTextBoxSelect == 0 || PassTextBox.Password == "Password"))
            {
                PassTextBox.Password = "";
                PassTextBox.Opacity = 1;
            }
            forFirstPassTextBoxSelect = 1;
        }

        private void PassTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PassTextBox.Password == "")
            {
                PassTextBox.Password = "Password";
                PassTextBox.Opacity = 0.5;
            }
        }

        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            closeButton.Foreground = Brushes.Red;
        }

        private void closeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeButton.Foreground = Brushes.White;
        }
        private void Registrate(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Registration registration = new Registration();
            registration.Show();
        }

        private void Authorizate(object sender, RoutedEventArgs e)
        {
            SqliteConnection connection = new SqliteConnection("Data Source=DataBase.db");
            connection.Open();
            string stm = "SELECT Login, Password FROM Users;";
            SqliteCommand cmd = new SqliteCommand(stm, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            List<string> logins = new List<string>();
            List<string> passwords = new List<string>();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    logins.Add(rdr.GetString(0));
                    passwords.Add(rdr.GetString(1));
                    //test+=rdr.GetString(0) + " " + rdr.GetString(1)+'\n';
                }
            }
            if (logins.FindIndex(x => x == UserTextBox.Text) != -1 && passwords[logins.FindIndex(x => x == UserTextBox.Text)] == PassTextBox.Password)
            {
                stm = $"SELECT * FROM Users where Login = '{logins.Find(x => x == UserTextBox.Text)}';";
                cmd = new SqliteCommand(stm, connection);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                this.Hide();
                if (rdr.GetString(9)=="True") {
                    AdminForm.current = new User(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetDateTime(7), rdr.GetString(8), rdr.GetBoolean(9));
                    (new AdminForm()).Show();   

                }
                else
                {
                    UserForm.current = new User(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetDateTime(7), rdr.GetString(8), rdr.GetBoolean(9));
                    (new UserForm()).Show();
                }
                connection.Close();

            }
            else
            {
                k++;
                if (k == 3)
                {
                    (new Dialogs.ProfileDialog("Вы ввели неправельный пароль более 3 раз", Brushes.Red)).Show();
                    this.Close();
                }
                else
                    (new Dialogs.ProfileDialog("Неправельный логин или пароль", Brushes.Red)).Show();

            }
        }

        private void Border_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.Close();
        }
    }
}
