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

namespace theater.PageEdit
{
    /// <summary>
    /// Логика взаимодействия для PageEditActor.xaml
    /// </summary>
    public partial class PageEditActor : Page
    {
        // Переменная для хранения данных текущего объекта спектакль
        private actors currentActor;

        // Переменная для пути к изображению на компьютере
        private string photoFilePath;

        public users objUser { get; set; }

        public PageEditActor(actors actors, users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            currentActor = actors;
            LoadActorData();
        }



        // Подгружаем данные о выбранном актёре
        private void LoadActorData()
        {
            editActorFIO.Text = currentActor.fio;
            editActorAge.Text = currentActor.age.ToString();
            cbSex.SelectedItem = currentActor.sex;
            editActorHeight.Text = currentActor.height.ToString();
            editActorWeight.Text = currentActor.weight.ToString();
            editActorDetails.Text = currentActor.contact_details;
        }



        // Кнопка загрузки изображение для предпросмотра
        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                photoFilePath = openFileDialog.FileName;
                addActorPhoto.Text = photoFilePath;

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
                addActorDescription.Text = fileDescription;

                // Отображение названия файла в поле photo1
                addActorPhoto1.Text = System.IO.Path.GetFileName(photoFilePath);
            }
        }



        // Кнопка сохранения изменений
        private void btnSaveEditActor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем в базу изменённые данные
                using (var context = new TheaterEntities10())
                {
                    var actorToUpdate = context.actors.FirstOrDefault(p => p.id_actor == currentActor.id_actor);
                    if (actorToUpdate  != null)
                    {
                        actorToUpdate.fio = editActorFIO.Text;
                        actorToUpdate.age = int.Parse(editActorAge.Text);
                        actorToUpdate.sex = (cbSex.SelectedItem as ComboBoxItem)?.Content.ToString();
                        actorToUpdate.height = int.Parse(editActorHeight.Text);
                        actorToUpdate.weight = int.Parse(editActorWeight.Text);
                        actorToUpdate.contact_details = editActorDetails.Text;

                        // Замена фотографии в том случае, если выбрана новая фотография
                        if (!string.IsNullOrEmpty(addActorPhoto1.Text))
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
                                MessageBox.Show("Файл заблокирован другим процессом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            // Проверка существования исходного файла
                            if (!File.Exists(photoFilePath))
                            {
                                MessageBox.Show("Фотография не найдена в указанной папке!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            // Копирование новой фотографии в папку "Images", если фотографии с таким названием ещё нет
                            if (!File.Exists(newPhotoFilePath))
                            {
                                File.WriteAllBytes(newPhotoFilePath, File.ReadAllBytes(photoFilePath));
                            }

                            string photoPath = System.IO.Path.GetFileName(photoFilePath);
                            string newPhoto1 = System.IO.Path.GetFileName(photoFilePath);
                            string newDescription = System.IO.Path.GetFileNameWithoutExtension(photoFilePath);

                            // Добавление новой фотографии в базу данных, если она ещё не добавлена
                            var newPhoto = context.photo.FirstOrDefault(p => p.photo1 == photoPath);
                            if (newPhoto == null)
                            {
                                newPhoto = new photo
                                {
                                    photo1 = newPhoto1,
                                    description = newDescription
                                };
                                context.photo.Add(newPhoto);
                                context.SaveChanges();
                            }

                            // Обновление id_photo в таблице performance 
                            actorToUpdate.id_photo = newPhoto.id_photo;
                        }
                        // Сохранение изменений в базе данных
                        context.SaveChanges();
                    }
                }

                // Уведомляем о успешном сохранении изменений и переходим на страницу PageListOfPerformance
                MessageBox.Show("Изменения успешно сохранены.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.Navigate(new PageActors(objUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageActors(objUser));
        }
    }
}
