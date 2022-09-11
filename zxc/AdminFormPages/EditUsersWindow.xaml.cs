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
namespace zxc.AdminFormPages
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class EditUsersWindow : Window
    {
        int k = 0;
        List<int> intForTextBoxPHPass;
        List<int> intForTextBoxPH;
        List<string> namesForTextBoxPH;
        List<string> stringsForTextBoxPH;
        public User user { get; set; }
        public bool isNew= false;
        public EditUsersWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            Binding myBinding = new Binding();
            myBinding.Source = user.Login;
            myBinding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(UserTextBox, TextBox.TextProperty, myBinding);
            UserTextBox.Opacity = 1;
            Binding myBinding1 = new Binding();
            myBinding1.Mode = BindingMode.OneWay;
            myBinding1.Source = user.Password;
            BindingOperations.SetBinding(PassTextBox1,TextBox.TextProperty, myBinding1);
            PassTextBox1.Opacity = 1;
            Binding myBinding2 = new Binding();
            myBinding2.Source = user.Email;
            myBinding2.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(EmailTextBox, TextBox.TextProperty, myBinding2);
            EmailTextBox.Opacity = 1;
            Binding myBinding3 = new Binding();
            myBinding3.Mode = BindingMode.OneWay;
            myBinding3.Source = user.Name;
            BindingOperations.SetBinding(NameTextBox, TextBox.TextProperty, myBinding3);
            NameTextBox.Opacity = 1;
            Binding myBinding4 = new Binding();
            myBinding4.Mode = BindingMode.OneWay;
            myBinding4.Source = user.Surname;
            BindingOperations.SetBinding(SurnameTextBox, TextBox.TextProperty, myBinding4);
            SurnameTextBox.Opacity = 1;
            Binding myBinding5 = new Binding();
            myBinding5.Mode = BindingMode.OneWay;
            myBinding5.Source = user.SecondaryName;
            BindingOperations.SetBinding(SNameTextBox, TextBox.TextProperty, myBinding5);
            SNameTextBox.Opacity = 1;
            Binding myBinding6 = new Binding();
            myBinding6.Mode = BindingMode.OneWay;
            myBinding6.Source = user.Telephone;
            BindingOperations.SetBinding(TelephoneTextBox, TextBox.TextProperty, myBinding6);
            TelephoneTextBox.Opacity = 1;
            Binding myBinding7 = new Binding();
            myBinding7.Mode = BindingMode.OneWay;
            myBinding7.Source = user.Date;
            BindingOperations.SetBinding(DateTextBox, TextBox.TextProperty, myBinding7);
            DateTextBox.Opacity = 1;
            Binding myBinding8 = new Binding();
            myBinding8.Mode = BindingMode.OneWay;
            myBinding8.Source = user.Adres;
            BindingOperations.SetBinding(AdresTextBox, TextBox.TextProperty, myBinding8);
            AdresTextBox.Opacity = 1;
            qq.Content = "Изменение даннных пользователя";
        }
        public EditUsersWindow()
        {
            InitializeComponent();
            isNew = true;
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
            button.Content = "Insert";
        }
        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            closeButton.Foreground = Brushes.Red;
        }

        private void closeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeButton.Foreground = Brushes.White;
        }
        private void UserTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                if (namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name) == -1)
                {
                    namesForTextBoxPH[k] = ((TextBox)sender).Name;
                    stringsForTextBoxPH[k] = ((TextBox)sender).Text;
                    k++;
                }
                if (((TextBox)sender).Text == null || (intForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)] == 0 || ((TextBox)sender).Text == stringsForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)]))
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Opacity = 1;
                    intForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)] = 1;
                }
            }
        }
        private void UserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                if (((TextBox)sender).Text == "")
                {
                    ((TextBox)sender).Text = stringsForTextBoxPH[namesForTextBoxPH.FindIndex(x => x == ((TextBox)sender).Name)];
                    ((TextBox)sender).Opacity = 0.5;
                }
            }
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
            if (logins.FindIndex(x => UserTextBox.Text == x) != -1 && UserTextBox.Text == AdminForm.current.Login)
            {
                (new Dialogs.ProfileDialog("Пользователь с таким логином уже есть", Brushes.Red)).ShowDialog();
                return;
            }
            if (UserTextBox.Text.Length < 4)
            {
                (new Dialogs.ProfileDialog("Логин должен быть от 4 символов", Brushes.Red)).ShowDialog();
                return;
            }
            if (PassTextBox1.Text.Length < 4)
            {
                (new Dialogs.ProfileDialog("Пароль должен быть от 4 символов", Brushes.Red)).ShowDialog();
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
            if (isNew)
            {
                connection = new SqliteConnection("Data Source=DataBase.db");
                connection.Open();
                stm = "SELECT Login FROM Users;";
                cmd = new SqliteCommand(stm, connection);
                rdr = cmd.ExecuteReader();
                logins = new List<string>();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        logins.Add(rdr.GetString(0));
                    }
                }
                connection.Close();
                if (logins.FindIndex(x => UserTextBox.Text == x) != -1)
                    (new Dialogs.ProfileDialog("Пользователь с таким логином уже есть", Brushes.Red)).ShowDialog();
                else
                {
                    connection.Open();
                    stm = $"INSERT INTO Users(Login,  Password,  Email, Surname,  Name,  [Secondary name],Adres,  Date,  Telephone,  isAdmin ) VALUES ('{UserTextBox.Text}', '{PassTextBox1.Text}', '{EmailTextBox.Text}', '{SurnameTextBox.Text}', '{NameTextBox.Text}', '{SNameTextBox.Text}', '{AdresTextBox.Text}', '{DateTextBox.Text}', '{TelephoneTextBox.Text}' , 'False');";
                    cmd = new SqliteCommand(stm, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    (new Dialogs.ProfileDialog("Учётная запись была успешно создана", Brushes.Lime)).ShowDialog();
                    this.DialogResult = true;
                    this.Close();
                }
            }
            else
            {
                connection = new SqliteConnection("Data Source=DataBase.db");
                connection.Open();
                stm = $"UPDATE Users SET Login = '{UserTextBox.Text}', Password = '{PassTextBox1.Text}' ,Email = '{EmailTextBox.Text}' ,Surname = '{SurnameTextBox.Text}' ,Name = '{NameTextBox.Text}' ,[Secondary name] = '{SNameTextBox.Text}', Adres = '{AdresTextBox.Text}',Date = '{DateTextBox.Text}',Telephone = '{TelephoneTextBox.Text}' WHERE Login = '{user.Login}'; ";
                cmd = new SqliteCommand(stm, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                (new Dialogs.ProfileDialog("Данные были успешно изменены", Brushes.Lime)).ShowDialog();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

