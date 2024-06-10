using Microsoft.Win32;
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

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddPerformance.xaml
    /// </summary>
    public partial class PageAddPerformance : Page
    {
        private string photoFileName;

        public PageAddPerformance()
        {
            InitializeComponent();
        }


        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.SelectedIndex != 0 && comboBox.Items[0] is ComboBoxItem placeholderItem && !placeholderItem.IsEnabled)
                {
                    comboBox.Items.RemoveAt(0);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageMenuAdmin());
        }

        private void btnAddPerformance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(photoFileName))
                {
                    MessageBox.Show("Выберите фотографию спектакля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string photoPath = Path.Combine("Images", photoFileName);

                if (!File.Exists(photoFileName))
                {
                    MessageBox.Show("Фотография не найдена в указанной папке!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photoPath);
                FileDialog.Copy(photoFileName, destinationPath, true);

                using (var context = new TheaterEntities6())
                {
                    

                    performance newPerformance = new performance
                    {
                        title = addPerformanceTitle.Text,
                        genre = (cbGenre.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        year_created = int.Parse(addPerformanceYearCreated.Text),
                        author = addPerformanceAuthor.Text,
                        duration = TimeSpan.Parse(addPerformanceDuration.Text),
                        id_photo = newPhoto.id_photo
                    };
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка при добавлении спектакля: {ex.Message}");
            }
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                photoFileName = openFileDialog.FileName;
                addPerformancePhoto.Text = photoFileName;
            }
        }
    }
}
