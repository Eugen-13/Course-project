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

namespace zxc
{
    /// <summary>
    /// Логика взаимодействия для PreLoad.xaml
    /// </summary>
    public partial class PreLoad : Window
    {
        System.Windows.Threading.DispatcherTimer myDispatcherTimer;
        public PreLoad()
        {
            InitializeComponent();
            myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            myDispatcherTimer.Tick += myDispatcherTimer_Tick;
            myDispatcherTimer.Start();
        }
        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            progres.Value += 1;
            procent.Content = progres.Value.ToString() + " %";
            if(progres.Value == 100)
            {
                MainWindow mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                myDispatcherTimer.Stop();
                Close();
            }
        }
    }
}
