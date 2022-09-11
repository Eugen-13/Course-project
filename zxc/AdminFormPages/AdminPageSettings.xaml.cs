using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
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
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using exportWord = Microsoft.Office.Interop.Word;

namespace zxc.AdminFormPages
{
    /// <summary>
    /// Логика взаимодействия для AdminPageSettings.xaml
    /// </summary>
    public partial class AdminPageSettings : System.Windows.Controls.Page
    {
        string current;
        List<User> users;
        List<Computers> computers;
        public AdminPageSettings()
        {
            InitializeComponent();
            SqliteConnection connection = new SqliteConnection("Data Source=DataBase.db");
            users = new List<User>();
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
            connection.Close();
            string text = "";
            foreach (var item in users)
            {
                text += item.ToString();
            }
            current = text;
            computers = new List<Computers>();
            connection.Open();
            s = "SELECT * FROM Computers;";
            cmd = new SqliteCommand(s, connection);
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    computers.Add(new Computers(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4)));

                }
            }
            connection.Close();
        }

        private void File(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Текстик (*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.Default))
                {
                    sw.Write(current);
                }
            }
        }

        private void EmailP(object sender, RoutedEventArgs e)
        {
            try
            {

                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("zheka137.soloveyv@mail.ru", "DCs7AHfE4MZ9UzXwWJij");
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("zheka137.soloveyv@mail.ru");
                Message.Subject = "Клиенты";
                Message.To.Add(new MailAddress(AdminForm.current.Email));
                Message.Body = current;
                smtp.Send(Message);
                (new Dialogs.ProfileDialog("Сообщение успещно отправлено", Brushes.Lime)).ShowDialog();
            }
            catch (Exception)
            {
                (new Dialogs.ProfileDialog("Ошибка отправки сообщения", Brushes.Red)).ShowDialog();
            }

        }

        private void Print(object sender, RoutedEventArgs e)
        {

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                switch (((ComboBoxItem)combo.SelectedItem).Content.ToString())
                {
                    case "Users":
                        printDialog.PrintVisual(new AdminPageClientsxaml().Data, "Users");
                        break;
                    case "Computers":
                        printDialog.PrintVisual(new UserFormPages.UserFormPagePlaces().Data, "Computers");
                        break;
                }

            }
        }

        private void Helpp(object sender, RoutedEventArgs e)
        {
            (new Dialogs.Help()).ShowDialog();
        }

        private void Excel(object sender, RoutedEventArgs e)
        {
                if (((ComboBoxItem)combo.SelectedItem).Content.ToString() == "Computers")
                {
                    DataGrid dataGrid = new UserFormPages.UserFormPagePlaces().Data;
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    excel.Visible = true;
                    Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                    Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet1.Cells[1, j + 1];
                        sheet1.Cells[1, j + 1].Font.Bold = true;
                        sheet1.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = dataGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < dataGrid.Items.Count; j++)
                        {
                            TextBlock b = dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                            if (i == 0)
                                myRange.Value2 = (dataGrid.Items[j] as Computers).ID;
                            if (i == 1)
                                myRange.Value2 = (dataGrid.Items[j] as Computers).Class;
                            if (i == 2)
                                myRange.Value2 = (dataGrid.Items[j] as Computers).Cost;
                            if (i == 3)
                                myRange.Value2 = (dataGrid.Items[j] as Computers).Stats;
                            if (i == 4)
                                myRange.Value2 = (dataGrid.Items[j] as Computers).Devices;
                        }
                    }
                }
                else if (((ComboBoxItem)combo.SelectedItem).Content.ToString() == "Users")
                {
                    DataGrid dataGrid = new AdminFormPages.AdminPageClientsxaml().Data;
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    excel.Visible = true;
                    Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                    Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet1.Cells[1, j + 1];
                        sheet1.Cells[1, j + 1].Font.Bold = true;
                        sheet1.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = dataGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < dataGrid.Items.Count; j++)
                        {
                            TextBlock b = dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                            if (i == 0)
                                myRange.Value2 = (dataGrid.Items[j] as User).Login;
                            if (i == 1)
                                myRange.Value2 = (dataGrid.Items[j] as User).Password;
                            if (i == 2)
                                myRange.Value2 = (dataGrid.Items[j] as User).Email;
                            if (i == 3)
                                myRange.Value2 = (dataGrid.Items[j] as User).Name;
                            if (i == 4)
                                myRange.Value2 = (dataGrid.Items[j] as User).Surname;
                            if (i == 5)
                                myRange.Value2 = (dataGrid.Items[j] as User).Date;
                            if (i == 6)
                                myRange.Value2 = $"'{(dataGrid.Items[j] as User).Telephone}'";
                            if (i == 7)
                                myRange.Value2 = (dataGrid.Items[j] as User).Adres;
                        }
                    }
                } 
        }

        private void Word(object sender, RoutedEventArgs e)
        {
            (new Thread(() =>
            {
                exportWord.Application wordapp = new exportWord.Application();
                wordapp.Visible = true;
                exportWord.Document worddoc;
                object wordobj = System.Reflection.Missing.Value;
                worddoc = wordapp.Documents.Add(ref wordobj);
                wordapp.Selection.TypeText(current);
                wordapp = null;
            })).Start();
        

        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exc == null)
                return;
            switch (((ComboBoxItem)combo.SelectedItem).Content.ToString())
            {
                
                case "Profile":
                    exc.Visibility = Visibility.Hidden;
                    print.Visibility = Visibility.Hidden;
                    current = AdminForm.current.ToString();
                    break;
                default:
                    if (((ComboBoxItem)combo.SelectedItem).Content.ToString() == "Computers")
                    {
                        string text = "";
                        foreach (var item in computers)
                        {
                            text += item.ToString();
                        }
                        current = text;
                    }
                    else if(((ComboBoxItem)combo.SelectedItem).Content.ToString() == "Users")
                    {
                        string text = "";
                        foreach (var item in users)
                        {
                            text += item.ToString();
                        }
                        current = text;
                    }
                    exc.Visibility = Visibility.Visible;
                    print.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
