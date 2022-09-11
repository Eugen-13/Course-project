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

namespace zxc.AdminFormPages
{
    /// <summary>
    /// Логика взаимодействия для AdminPageExit.xaml
    /// </summary>
    public partial class AdminPageExit : Page
    {
        Window admin;
        public AdminPageExit(Window admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            admin.Close();
            Application.Current.MainWindow.Show();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            admin.Close();
            Application.Current.MainWindow.Close();
        }
    }
}
