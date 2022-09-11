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
using System.Threading;
namespace zxc.UserFormPages
{
    /// <summary>
    /// Логика взаимодействия для UserFormPagePlaces.xaml
    /// </summary>
    public partial class UserFormPagePlaces : Page
    {
        bool b = false;
        int forFirstUserTextBoxSelect = 0;
        SqliteConnection connection;
        private static void ExecuteNonQuery(string queryString)
        {

            using (var connection = new SqliteConnection("Data Source=DataBase.db"))
            {
                using (var command = new SqliteCommand(queryString, connection))
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public ObservableCollection<Computers> computers { get; set; }
        public void Read()
        {
            List<Requests> requests = new List<Requests>();
            string s = $"SELECT * FROM Requests where isAccept = 'False';";
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
            computers = new ObservableCollection<Computers>();
            connection.Close();
            connection.Open();
            s = "SELECT * FROM Computers;";
            cmd = new SqliteCommand(s, connection);
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    if (requests.FindIndex(x => x.Computer == rdr.GetInt32(0)) == -1)
                    {
                        computers.Add(new Computers(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4)));
                    }
                }
            }
            Data.ItemsSource = null;

            Data.ItemsSource = computers;

            connection.Close();
        }
        public void Read(string s)
        {
            computers = new ObservableCollection<Computers>();
            connection.Open();
            SqliteCommand cmd = new SqliteCommand(s, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    computers.Add(new Computers(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4)));

                }
            }
            Data.ItemsSource = null;

            Data.ItemsSource = computers;

            connection.Close();
        }
        public UserFormPagePlaces()
        {
            InitializeComponent();

            connection = new SqliteConnection("Data Source=DataBase.db");
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
                Read($"SELECT * FROM Computers where {value} Like '%{UserTextBox.Text}%';");
                b1.Opacity = 0.5;
                b1.IsEnabled = false;
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Requests> requests = new ObservableCollection<Requests>();
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
            connection.Close();
            if(requests.Any(x=> x.isAccept == "False" && x.Login == UserForm.current.Login))
            {
                (new Dialogs.ProfileDialog("У вас уже есть активная заявка", Brushes.Red)).ShowDialog();
                return;
            }
            List<int> ids = new List<int>();
            connection.Open();
            s = "SELECT ID FROM Requests;";
            cmd = new SqliteCommand(s, connection);
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    ids.Add(rdr.GetInt32(0));
                }
            }
            connection.Close();
            int id = (Data.SelectedItem as Computers).ID;
            int number = 0;
            if (ids.Count >0)
            number = ids.Max() + 1;
            s = $" INSERT INTO Requests( ID, User, Computer, Date,  isAccept ) VALUES ( '{number}', '{UserForm.current.Login}', '{id}', '{DateTime.Now.ToString("yyyy.MM.dd")}','False'); ";
            ExecuteNonQuery(s);
            Read();
            (new Dialogs.ProfileDialog("Заявка  была  успешно  отправлена", Brushes.Linen)).ShowDialog();         
        }
        private void Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            b1.Opacity = 1;
            b1.IsEnabled = true;
        }
    }
   
}
