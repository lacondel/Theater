using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using theater.ApplicationData;
using theater.PageMain;

namespace theater.PageEdit
{
    /// <summary>
    /// Логика взаимодействия для PageEditPerformance.xaml
    /// </summary>
    public partial class PageEditPerformance : Page
    {
        // Переменная для хранения данных текущего объекта спектакль
        private performance currentPerformance;

        // Переменная для пути к изображению на компьютере
        private string photoFilePath;

        public users objUser { get; set; }

        public PageEditPerformance(performance performance, users user)
        {
            this.objUser = objUser;
            InitializeComponent();
            currentPerformance = performance;
            LoadPerformanceData();
        }

        private void LoadPerformanceData()
        {
            editPerformanceTitle.Text = currentPerformance.title;
            cbGenre.SelectedItem= currentPerformance.genre;
            editPerformanceYearCreated.Text = currentPerformance.year_created.ToString();
            editPerformanceAuthor.Text = currentPerformance.author;
            editPerformanceDuration.Text = currentPerformance.duration.ToString(@"hh\:mm");
        }


        // Кнопка загрузки изображение для предпросмотра
        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                photoFilePath = openFileDialog.FileName;
                addPerformancePhoto.Text = photoFilePath;

                // Загрузка изображения для предварительного просмотра
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(photoFilePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgPhotoPreview.Source = bitmap;

                // Отображение названия файла в поле description
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(photoFilePath);
                string fileDescription = string.Join(" ", fileNameWithoutExtension.Split('_')); // Пример разделения имени файла
                addPerformanceDescription.Text = fileDescription;

                // Отображение названия файла в поле photo1
                addPerformancePhoto1.Text = System.IO.Path.GetFileName(photoFilePath);
            }
        }

        // Кнопка сохранения изменений
        private void btnEditPerformance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем в базу изменённые данные
                using (var context = new TheaterEntities7())
                {
                    var performanceToUpdate = context.performance.FirstOrDefault(p => p.id_performance == currentPerformance.id_performance);
                    if (performanceToUpdate != null)
                    {
                        performanceToUpdate.title = editPerformanceTitle.Text;
                        performanceToUpdate.genre = (cbGenre.SelectedItem as ComboBoxItem)?.Content.ToString();
                        performanceToUpdate.year_created = int.Parse(editPerformanceYearCreated.Text);
                        performanceToUpdate.author = editPerformanceAuthor.Text;
                        performanceToUpdate.duration = TimeSpan.Parse(editPerformanceDuration.Text);

                        // Замена фотографии в том случае, если выбрана новая фотография
                        if (!string.IsNullOrEmpty(addPerformancePhoto1.Text)) 
                        { 
                            // Переменная хранящая путь до каталога приложения "theater"
                            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                            // Переменная хранящая путь до каталога "Images"
                            string imagesDirectory = System.IO.Path.Combine(projectDirectory, "Images");
                            // Переменная хранящая путь для сохранения файла нового изображения
                            string newPhotoFilePath = System.IO.Path.Combine(imagesDirectory, System.IO.Path.GetFileName(photoFilePath));

                            // Проверка блокировки файла
                            if (MethodsForView.IsFileLocked(new FileInfo(photoFilePath)))
                            {
                                MessageBox.Show("Файд заблокирован другим процессом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            // Проверка существования исходного файла
                            if (!File.Exists(photoFilePath))
                            {
                                MessageBox.Show("Фотография не найдена в указанной папке!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            // Копирование новой фотографии в папку "Images"
                            File.WriteAllBytes(newPhotoFilePath, File.ReadAllBytes(photoFilePath));

                            // Добавление новой фотографии в базу данных, если она ещё не добавлена
                            var newPhoto = context.photo.FirstOrDefault(p => p.photo1 == System.IO.Path.GetFileName(photoFilePath));
                            if (newPhoto == null) 
                            {
                                newPhoto = new photo
                                {
                                    photo1 = System.IO.Path.GetFileName(photoFilePath),
                                    description = System.IO.Path.GetFileNameWithoutExtension(photoFilePath)
                                };
                                context.photo.Add(newPhoto);
                                context.SaveChanges();
                            }

                            // Обновление id_photo в таблице performance 
                            performanceToUpdate.id_photo = newPhoto.id_photo;

                            // Сохранение изменений в базе данных
                            context.SaveChanges();
                        }

                        context.SaveChanges();
                    }
                }

                // Уведомляем о успешном сохранении изменений и переходим на страницу PageListOfPerformance
                MessageBox.Show("Изменения успешно сохранены.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.Navigate(new PageListOfPerformances(objUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageListOfPerformances(objUser));
        }
    }
}
