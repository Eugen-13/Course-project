using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace zxc.AdminFormPages
{
    /// <summary>
    /// Логика взаимодействия для AdminPageClientsxaml.xaml
    /// </summary>
    public partial class AdminPageClientsxaml : Page
    {
        bool b = false;
        int forFirstUserTextBoxSelect = 0;
        SqliteConnection connection;
        public ObservableCollection<User> users { get; set; }    
        public void Read()
        {
            users = new ObservableCollection<User>();
            connection.Open();
            string s = "SELECT * FROM Users where isAdmin = 'False';";
            SqliteCommand cmd = new SqliteCommand(s, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    users.Add(new User(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetDateTime(7), rdr.GetString(8), rdr.GetBoolean(9)));

                }
            }
            Data.ItemsSource = null;

            Data.ItemsSource = users;

            connection.Close();
        }
        public void Read(string s)
        {
            users = new ObservableCollection<User>();
            connection.Open();
            SqliteCommand cmd = new SqliteCommand(s, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    users.Add(new User(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetDateTime(7), rdr.GetString(8), rdr.GetBoolean(9)));

                }
            }
            Data.ItemsSource = null;

            Data.ItemsSource = users;

            connection.Close();
        }
        public AdminPageClientsxaml()
        {
            InitializeComponent();
            connection = new SqliteConnection("Data Source=DataBase.db");
            Read();
        }

        private void Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            b1.Opacity = 1;
            b2.Opacity = 1;
            b1.IsEnabled = true;
            b2.IsEnabled = true;
           
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            string login = (Data.SelectedItem as User).Login;
            if ((new Dialogs.AcceptPage("Вы  действительно  хотите  удалить  пользователя")).ShowDialog() == true)
            {
                connection.Open();
                string s = $"DELETE FROM Users WHERE Login = '{login}'";
                SqliteCommand cmd = new SqliteCommand(s, connection);
                cmd.ExecuteReader();
                connection.Close();
                Read();
            }
        }
        private void Change(object sender, RoutedEventArgs e)
        {
            EditUsersWindow edit = new EditUsersWindow((Data.SelectedItem as User));
            if (edit.ShowDialog() == true)
                Read();
        }

        private void Insert(object sender, RoutedEventArgs e)
        {
            EditUsersWindow edit = new EditUsersWindow();
            if (edit.ShowDialog() == true)
                Read();

        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text == null || (forFirstUserTextBoxSelect == 0 || UserTextBox.Text == "Search"))
            {
                UserTextBox.Text = "";
                b = true;
                UserTextBox.Opacity = 1;
            }
            forFirstUserTextBoxSelect = 1;
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text == "")
            {
                b=false;
                UserTextBox.Text = "Search";
                UserTextBox.Opacity = 0.5;
            }
        }

        private void fields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserTextBox.IsEnabled = true;
        }

        private void Searcn(object sender, TextChangedEventArgs e)
        {
            if (b)
            {
                ComboBoxItem typeItem = (ComboBoxItem)fields.SelectedItem;
                string value = typeItem.Content.ToString();
                Read($"SELECT * FROM Users where {value} Like '%{UserTextBox.Text}%';");
                b1.Opacity = 0.5;
                b1.IsEnabled = false;
                b2.Opacity = 0.5;
                b2.IsEnabled = false;
            }
        }
    }
}
