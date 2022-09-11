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
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
namespace zxc
{
  
    public partial class Registration : Window
    {
        int k = 0;
        List<int> intForTextBoxPHPass;
        List<int> intForTextBoxPH;
        List<string> namesForTextBoxPH;
        List<string> stringsForTextBoxPH;
        public Registration()
        {
            InitializeComponent();
            intForTextBoxPH = new List<int>();
            namesForTextBoxPH = new List<string>();
            stringsForTextBoxPH = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                intForTextBoxPH.Add(0);
                namesForTextBoxPH.Add("");
                stringsForTextBoxPH.Add("");
            }
            intForTextBoxPHPass = new List<int>();
            for (int i = 0; i < 2; i++)
                  intForTextBoxPHPass.Add(0);
            }

        private void UserTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name) == -1)
            {
                namesForTextBoxPH[k] = ((TextBox)sender).Name;
                stringsForTextBoxPH[k] = ((TextBox)sender).Text;
                k++;
            }
            if (((TextBox)sender).Text == null || (intForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)] == 0 || ((TextBox)sender).Text == stringsForTextBoxPH[namesForTextBoxPH.FindIndex(x =>x == ((TextBox)sender).Name)]))
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Opacity = 1;
                intForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)] = 1;
            }
           
        }
        private void UserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Text = stringsForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)];
                ((TextBox)sender).Opacity = 0.5;
            }
        }

        private void PassTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            if (((PasswordBox)sender).Name == "PassTextBox1")
            {
                if ((((PasswordBox)sender)).Password == null || (intForTextBoxPHPass[0] == 0 || ((PasswordBox)sender).Password == "Password"))
                {
                    ((PasswordBox)sender).Password = "";
                    ((PasswordBox)sender).Opacity = 1;
                }
                intForTextBoxPHPass[0] = 1;
            }
            else
            {
                if ((((PasswordBox)sender)).Password == null || (intForTextBoxPHPass[1] == 0 || ((PasswordBox)sender).Password == "Password"))
                {
                    ((PasswordBox)sender).Password = "";
                    ((PasswordBox)sender).Opacity = 1;
                }
                    intForTextBoxPHPass[1] = 1;
            }
        }

        private void PassTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).Password == "")
            {
                ((PasswordBox)sender).Password = "Password";
                ((PasswordBox)sender).Opacity = 0.5;
            }
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }

        private void Back_MouseEnter(object sender, MouseEventArgs e)
        {
            backButton.Foreground = Brushes.Red;
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            backButton.Foreground = Brushes.White;
        }

        private void UserTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        bool Letters(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsLetter(s[i]))
                    return false;
            }
            return true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqliteConnection connection = new SqliteConnection("Data Source=DataBase.db");
            connection.Open();
            string stm = "SELECT Login FROM Users;";
            SqliteCommand cmd = new SqliteCommand(stm, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            List<string> logins = new List<string>();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    logins.Add(rdr.GetString(0));
                }
            }
            connection.Close();
            string pattern = @"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$";
            if (logins.FindIndex(x => UserTextBox.Text == x) != -1)
            {
                (new Dialogs.ProfileDialog("Пользователь с таким логином уже есть", Brushes.Red)).ShowDialog();
                return;
            }
            if (UserTextBox.Text.Length < 4)
            {
                (new Dialogs.ProfileDialog("Логин должен быть от 4 символов", Brushes.Red)).ShowDialog();
                return;
            }
            if (PassTextBox1.Password.Length < 4)
            {
                (new Dialogs.ProfileDialog("Пароль должен быть от 4 символов", Brushes.Red)).ShowDialog();
                return;
            }
            if (PassTextBox1.Password != PassTextBox2.Password)
            {
                (new Dialogs.ProfileDialog("Пароли не совпадают", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Regex.IsMatch(EmailTextBox.Text, pattern))
            {
                (new Dialogs.ProfileDialog("Неверный формат email", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Letters(SurnameTextBox.Text))
            {
                (new Dialogs.ProfileDialog("Неверный формат фамилии", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Letters(NameTextBox.Text))
            {
                (new Dialogs.ProfileDialog("Неверный формат имени", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Letters(SNameTextBox.Text))
            {
                (new Dialogs.ProfileDialog("Неверный формат отчества", Brushes.Red)).ShowDialog();
                return;
            }
            int b = 0;
            string[] st = DateTextBox.Text.Split('.');
            int year = 0;
            int month = 0;
            int day = 0;
            try
            {
                year = int.Parse(st[0]);
                month = int.Parse(st[1]);
                day = int.Parse(st[2]);
            }
            catch (Exception)
            {
                (new Dialogs.ProfileDialog("Неверный формат даты (yyyy.mm.dd)", Brushes.Red)).ShowDialog();
                return;
            }

            if (year < 1900 || year > DateTime.Now.Year)
                b = 1;
            if (month < 1 || month > 12)
                b = 1;
            if (day < 1 || day > 31)
                b = 1;
            int yy = 28;
            if (yy % 4 == 0)
                yy = 29;
            if (day > yy)
                b = 1;
            if (b == 1)
            {
                (new Dialogs.ProfileDialog("Неверный формат даты (yyyy.mm.dd)", Brushes.Red)).ShowDialog();
                return;
            }
            pattern = @"^\+375\(17|25|29|33|44|25\)[0-9]{7}";
            if (!Regex.IsMatch(TelephoneTextBox.Text, pattern) || TelephoneTextBox.Text.Length != 13)
            {
                (new Dialogs.ProfileDialog("Неверный формат телефона (+375xxxxxxxxx)", Brushes.Red)).ShowDialog();
                return;
            }
            connection.Open();
            stm = $"INSERT INTO Users(Login,  Password,  Email, Surname,  Name,  [Secondary name],Adres,  Date,  Telephone,  isAdmin ) VALUES ('{UserTextBox.Text}', '{PassTextBox1.Password}', '{EmailTextBox.Text}', '{SurnameTextBox.Text}', '{NameTextBox.Text}', '{SurnameTextBox.Text}', '{AdresTextBox.Text}', '{DateTextBox.Text}', '{TelephoneTextBox.Text}' , 'False');";
            cmd = new SqliteCommand(stm, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            (new Dialogs.ProfileDialog("Учётная запись была успешно создана", Brushes.Lime)).ShowDialog();
            this.Close();
            Application.Current.MainWindow.Show();
                   
            
        }
    }
}
