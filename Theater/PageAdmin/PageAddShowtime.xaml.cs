using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для PageAddShowtime.xaml
    /// </summary>
    public partial class PageAddShowtime : Page
    {
        public users objUser { get; set; }

        // Переменная для пути к изображению на компьютере
        private string photoFilePath;

        public PageAddShowtime(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
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



        // Конопка добавления представления в базу данных
        private void btnAddShowtime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Переменная хранящая путь до каталога приложения "theater"
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                // Переменная хранящая путь до каталога "Images"
                string imagesDirectory = System.IO.Path.Combine(projectDirectory, "Images");
                // Переменная хранящая путь для сохранения файла нового изображения
                string photoPath = System.IO.Path.Combine(imagesDirectory, System.IO.Path.GetFileName(photoFilePath));

                // Проверка существования папки "Images" и создание её при необходимости
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                // Проверка блокировки файла
                if (MethodsForView.IsFileLocked(new FileInfo(photoFilePath)))
                {
                    MessageBox.Show("Файл заблокирован другим процессом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка существования исходного файла
                if (!File.Exists(photoFilePath))
                {
                    MessageBox.Show("Фотография не найдена в указанной папке!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Обнуление источника изображения для предварительного просмотра
                imgPhotoPreview.Source = null;

                // Копирование файла в папку "Images"
                File.WriteAllBytes(photoPath, File.ReadAllBytes(photoFilePath));

                // Добавление данных в базу данных
                using (var context = new TheaterEntities7())
                {
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

                    // Добавление представления в базу данных
                    showtime newShowtime = new showtime
                    {
                        id_performanсe = context.performance.FirstOrDefault(p => p.title == addPerformanceTitle.Text).id_performance,
                        date = DateTime.Parse(addShowtimeDate.Text),
                        price = decimal.Parse(addShowtimePrice.Text),
                        id_photo = newPhoto.id_photo
                    };
                    context.showtime.Add(newShowtime);
                    context.SaveChanges();
                }

                MessageBox.Show("Представление добавлено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении представления: {ex.Message}");
            }
        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes(objUser));
        }
    }
}
