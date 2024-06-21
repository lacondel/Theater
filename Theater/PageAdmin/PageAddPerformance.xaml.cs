﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using theater.ApplicationData;
using theater.PageMain;

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddPerformance.xaml
    /// </summary>
    public partial class PageAddPerformance : Page
    {
        public users objUser { get; set; }

        // Переменная для пути к изображению на компьютере
        private string photoFilePath;

        public PageAddPerformance(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
        }
        

        // Установка первого элемента в качестве выбранного при загрузке
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            MethodsForView.InitializeComboBox(comboBox);
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


        // Конопка добавления спектакля в базу данных
        private void btnAddPerformance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(photoFilePath))
                {
                    MessageBox.Show("Выберите фотографию спектакля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Определение пути к папке "Images" в корне проекта
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagesDirectory = System.IO.Path.Combine(projectDirectory, "Images");

                // Проверка существования папки "Images" и создание её при необходимости
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                // Определение полного пути для сохранения фотографии
                string photoPath = System.IO.Path.Combine(imagesDirectory, System.IO.Path.GetFileName(photoFilePath));

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

                // string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photoPath);

                // File.Copy(photoPath, destinationPath, true);

                using (var context = new TheaterEntities7())
                {
                    var newPhoto = new photo
                    { 
                        photo1 = System.IO.Path.GetFileName(photoFilePath),
                        description = System.IO.Path.GetFileNameWithoutExtension(photoFilePath)
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
                    context.performance.Add(newPerformance);
                    context.SaveChanges();
                }

                MessageBox.Show("Спектакль добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении спектакля: {ex.Message}");
            }
        }


        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageListOfPerformances(objUser));
        }
    }
}
