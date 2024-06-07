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

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddActor.xaml
    /// </summary>
    public partial class PageAddActor : Page
    {
        public PageAddActor()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null )
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null ) {
                if(comboBox.SelectedIndex != 0 && comboBox.Items[0] is ComboBoxItem placeholderItem && !placeholderItem.IsEnabled)
                {
                    comboBox.Items.RemoveAt(0);
                }
            }
        }
    }
}
