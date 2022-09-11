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

namespace zxc.UserFormPages
{
    /// <summary>
    /// Логика взаимодействия для UserFormPageRequests.xaml
    /// </summary>
    public partial class UserFormPageRequests : Page
    {
        bool b = false;
        int forFirstUserTextBoxSelect = 0;
        SqliteConnection connection;
        public DataGrid print;
        public ObservableCollection<Requests> requests { get; set; }
        public void Read()
        {
            requests = new ObservableCollection<Requests>();
            connection.Open();
            string s = $"SELECT * FROM Requests where User = '{UserForm.current.Login}';";
            SqliteCommand cmd = new SqliteCommand(s, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    requests.Add(new Requests(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetDateTime(3), rdr.GetString(4)));

                }
            }
            Data.ItemsSource = null;

            Data.ItemsSource = requests;

            connection.Close();
        }
        public void Read(string s)
        {
            requests = new ObservableCollection<Requests>();
            connection.Open();
            SqliteCommand cmd = new SqliteCommand(s, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    requests.Add(new Requests(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetDateTime(3), rdr.GetString(4)));

                }
            }
            Data.ItemsSource = null;

            Data.ItemsSource = requests;

            connection.Close();
        }
        public UserFormPageRequests()
        {
            InitializeComponent();

            connection = new SqliteConnection("Data Source=DataBase.db");
            Read();
            print = Data;
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
                b = false;
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
                Read($"SELECT * FROM Requests where {value} Like '%{UserTextBox.Text}%' AND  User = '{UserForm.current.Login}';");
            }
        }
    }
}
