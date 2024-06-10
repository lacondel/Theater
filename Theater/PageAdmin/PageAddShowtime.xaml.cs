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
using System.Windows.Navigation;
using System.Windows.Shapes;
using theater.ApplicationData;

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddShowtime.xaml
    /// </summary>
    public partial class PageAddShowtime : Page
    {
        public PageAddShowtime()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageMenuAdmin());
        }
    }
}