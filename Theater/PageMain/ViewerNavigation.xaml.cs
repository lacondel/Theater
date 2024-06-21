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
        public users objUser {  get; set; }

        public ViewerNavigation(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
        }

        private void btnShowtimes_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes(objUser));
        }

        private void btnPerformances_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageListOfPerformances(objUser));
        }

        private void btnActors_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageActors(objUser));
        }
    }
}
