using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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


        
        /// <summary>
        /// Методы для загрузки и изменения выбора в ComboBox
        /// </summary>
        
        // Установка первого элемента в качестве выбранного при загрузке
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.SelectedIndex = 0;
            }
        }
        
        // 
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

                string photoPath = System.IO.Path.Combine("Images", photoFileName);

                if (IsFileLocked(new FileInfo(photoFileName)))
                {
                    MessageBox.Show("Файл заблокирован другим процессом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!File.Exists(photoFileName))
                {
                    MessageBox.Show("Фотография не найдена в указанной папке!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                imgPhotoPreview.Source = null;

                string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photoPath);
                
                File.WriteAllBytes(destinationPath, File.ReadAllBytes(photoFileName));

                using (var context = new TheaterEntities6())
                {
                    var newPhoto = new photo
                    { 
                        photo1 = photoPath, 
                        description = photoPath
                    };
                    context.photo.Add(newPhoto);
                    context.SaveChanges();

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

                // Загрузка изображения для предварительного просмотра
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(photoFileName);
                bitmap.EndInit();
                imgPhotoPreview.Source = bitmap;

                // Отображение названия файла в поле description
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(photoFileName);
                string fileDescription = string.Join(" ", fileNameWithoutExtension.Split('_')); // Пример разделения имени файла
                addPerformanceDescription.Text = fileDescription;

                // Отображение названия файла в поле photo1
                addPerformancePhoto1.Text = System.IO.Path.GetFileName(photoFileName);
            }
        }

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                MessageBox.Show("Файл заблокирован другим процессом");
                return true;
            }
            finally
            {
                stream?.Close();
            }        

            return false;
        }
    }
}
