using System.Windows;
using theater.ApplicationData;
using theater.PageMain;

namespace theater
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            AppConnect.model0db = new TheaterEntities10();
            AppFrame.frameMain = FrmMain;

            FrmMain.Navigate(new PageLogin());
        }
    }
}