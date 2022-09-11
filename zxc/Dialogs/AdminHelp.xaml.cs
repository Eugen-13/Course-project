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

namespace zxc.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AdminHelp.xaml
    /// </summary>
    public partial class AdminHelp : Window
    {
        public AdminHelp()
        {
            InitializeComponent();
        }

        private void Help1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
