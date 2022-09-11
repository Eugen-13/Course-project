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
using System.Text.RegularExpressions;

namespace zxc.UserFormPages
{
    /// <summary>
    /// Логика взаимодействия для UserFormPageProfile.xaml
    /// </summary>
    public partial class UserFormPageProfile : Page
    {
        public UserFormPageProfile()
        {
            InitializeComponent();
            DataContext = UserForm.current;

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
            if (logins.FindIndex(x => login.Text == x) != -1 && login.Text != UserForm.current.Login)
            {
                (new Dialogs.ProfileDialog("Пользователь с таким логином уже есть", Brushes.Red)).ShowDialog();
                return;
            }
            if (login.Text.Length < 4)
            {
                (new Dialogs.ProfileDialog("Логин должен быть от 4 символов", Brushes.Red)).ShowDialog();
                return;
            }
            if (pass.Text.Length < 4)
            {
                (new Dialogs.ProfileDialog("Пароль должен быть от 4 символов", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Regex.IsMatch(email.Text, pattern))
            {
                (new Dialogs.ProfileDialog("Неверный формат email", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Letters(surname.Text))
            {
                (new Dialogs.ProfileDialog("Неверный формат фамилии", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Letters(name.Text))
            {
                (new Dialogs.ProfileDialog("Неверный формат имени", Brushes.Red)).ShowDialog();
                return;
            }
            if (!Letters(SName.Text))
            {
                (new Dialogs.ProfileDialog("Неверный формат отчества", Brushes.Red)).ShowDialog();
                return;
            }
            int b = 0;
            string[] st = date.Text.Split('.');
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
            if (!Regex.IsMatch(tel.Text, pattern) || tel.Text.Length != 13)
            {
                (new Dialogs.ProfileDialog("Неверный формат телефона (+375xxxxxxxxx)", Brushes.Red)).ShowDialog();
                return;
            }
            Dialogs.ProfileDialog profileDialog = new Dialogs.ProfileDialog();
            profileDialog.ShowDialog();
            connection = new SqliteConnection("Data Source=DataBase.db");
            connection.Open();
            stm = $"UPDATE Users SET Login = '{login.Text}', Password = '{pass.Text}' ,Email = '{email.Text}' ,Surname = '{surname.Text}' ,Name = '{name.Text}' ,[Secondary name] = '{SName.Text}', Adres = '{adres.Text}',Date = '{date.Text}',Telephone = '{tel.Text}' WHERE Login = '{UserForm.current.Login}'; ";
            cmd = new SqliteCommand(stm, connection);
            cmd.ExecuteNonQuery();
            connection.Close();

            connection.Open();
            stm = $"UPDATE Requests SET User = '{login.Text}' WHERE User = '{UserForm.current.Login}'; ";
            cmd = new SqliteCommand(stm, connection);
            cmd.ExecuteNonQuery();
            connection.Close();

            UserForm.current = new User(login.Text, pass.Text, email.Text, surname.Text, name.Text, SName.Text, adres.Text, DateTime.Parse(date.Text), tel.Text, false);
            Button_Click_1(sender, e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            DataContext = UserForm.current;
        }
    }
}
