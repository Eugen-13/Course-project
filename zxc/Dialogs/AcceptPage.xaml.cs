﻿using System;
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
    /// Логика взаимодействия для AcceptPage.xaml
    /// </summary>
    public partial class AcceptPage : Window
    {
        public AcceptPage()
        {
            InitializeComponent();
        }
        public AcceptPage(string str)
        {
            InitializeComponent();
            this.label.Content = str;   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.DialogResult = true;
                this.Close();
            }
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}