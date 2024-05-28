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
    /// Логика взаимодействия для PageActors.xaml
    /// </summary>
    public partial class PageActors : Page
    {
        public PageActors()
        {
            InitializeComponent();
            listOfActors.ItemsSource = AppConnect.model0db.actors.ToList();
            listOfActors.ItemsSource = AppConnect.model0db.photo.ToList();
        }
    }
}
