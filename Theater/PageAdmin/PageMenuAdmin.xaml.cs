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
using theater.ApplicationData;
using theater.PageMain;

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageMenuAdmin.xaml
    /// </summary>
    public partial class PageMenuAdmin : Page
    {
        public PageMenuAdmin()
        {
            InitializeComponent();
        }

        private void btnAddActor_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddActor());
        }

        private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddAdmin());
        }

        private void btnAddPerformance_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddPerformance());
        }

        private void btnAddShowtime_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddShowtime());
        }
    }
}
