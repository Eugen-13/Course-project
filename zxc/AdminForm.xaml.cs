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
using MahApps.Metro.IconPacks;

namespace zxc
{
    /// <summary>
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : Window
    {
        int k = 0;
        static public User current { get; set; }
        public AdminForm()
        {
           InitializeComponent();
            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            myDispatcherTimer.Tick += myDispatcherTimer_Tick;
            myDispatcherTimer.Start();
            Main.Content = new AdminFormPages.AdminFormStartPage();
        }
        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToString("HH:mm:ss");
            time1.Content = DateTime.Now.ToString("D");
        }
        private void Page1(object sender, RoutedEventArgs e)
        {
            Main.Content = new AdminFormPages.AdminPageProfile();
        }
        private void Page2(object sender, RoutedEventArgs e)
        {
            Main.Content = new AdminFormPages.AdminPageRequests();
        }
        private void Page3(object sender, RoutedEventArgs e)
        {
            Main.Content = new AdminFormPages.AdminPageClientsxaml();
        }

        private void PackIconMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            switch (k)
            {
                case 0:
                    ((PackIconMaterial)sender).Kind = PackIconMaterialKind.Account;
                    break;
                case 1:
                    ((PackIconMaterial)sender).Kind = PackIconMaterialKind.FormatListBulleted;
                    break;
                case 2:
                    ((PackIconMaterial)sender).Kind = PackIconMaterialKind.AccountMultiple;
                    break;
                case 3:
                    ((PackIconMaterial)sender).Kind = PackIconMaterialKind.Printer;
                    break;
                case 4:
                    ((PackIconMaterial)sender).Kind = PackIconMaterialKind.ExitToApp;
                    break;
            }
            k++;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Main.Content = new AdminFormPages.AdminPageExit(this);
            
        }

        private void SettingsPage(object sender, RoutedEventArgs e)
        {
            Main.Content = new AdminFormPages.AdminPageSettings();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (new Dialogs.Help()).ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            (new Dialogs.AdminHelp()).ShowDialog();
        }
    }
}
