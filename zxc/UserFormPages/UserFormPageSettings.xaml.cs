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
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System.Net.Mail;
using System.Net;
using Microsoft.Office.Interop.Excel;
using exportWord = Microsoft.Office.Interop.Word;
using System.Threading;

namespace zxc.UserFormPages
{
    /// <summary>
    /// Логика взаимодействия для UserFormPageSettings.xaml
    /// </summary>
    public partial class UserFormPageSettings : System.Windows.Controls.Page
    {
        List<Computers> computers;
        string current;
        public UserFormPageSettings()
        {
            InitializeComponent();
            SqliteConnection connection = new SqliteConnection("Data Source=DataBase.db");
            computers = new List<Computers>();
            connection.Open();
            string s = "SELECT * FROM Computers;";
            SqliteCommand cmd = new SqliteCommand(s, connection);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    computers.Add(new Computers(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4)));

                }
            }
            connection.Close();
            string text = "";
            foreach (var item in computers)
            {
                text += item.ToString();
            }
            current = text;
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
                Message.To.Add(new MailAddress(UserForm.current.Email));
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
                printDialog.PrintVisual(new UserFormPages.UserFormPagePlaces().Data, "Computers");
            }
        }

        private void Helpp(object sender, RoutedEventArgs e)
        {
            (new Dialogs.Help()).ShowDialog();
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
        private void Excel(object sender, RoutedEventArgs e)
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
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exc == null)
                return;
            switch (((ComboBoxItem)combo.SelectedItem).Content.ToString())
            {

                case "Profile":
                    exc.Visibility = Visibility.Hidden;
                    print.Visibility = Visibility.Hidden;
                    current = UserForm.current.ToString();
                    break;
                default:
                    string text = "";
                    foreach (var item in computers)
                    {
                        text += item.ToString();
                    }
                    current = text;
                    exc.Visibility = Visibility.Visible;
                    print.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
