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

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для ViewerNavigation.xaml
    /// </summary>
    public partial class ViewerNavigation : Page
    {
        public ViewerNavigation()
        {
            InitializeComponent();
        }

        private void btnShowtimes_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes());
        }

        private void btnPerformances_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageListOfPerformances());
        }

        private void btnActors_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageActors());
        }

        private void btnSponsors_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageSponsors());
        }
    }
}
