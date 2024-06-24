using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для PageAddActor.xaml
    /// </summary>
    public partial class PageAddActor : Page
    {
        public users objUser { get; set; }

        // Переменная для пути к изображению на компьютере
        private string photoFilePath;

        public PageAddActor(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            // Добавляем обработчик события SelectionChanged для ComboBox cbSex
            cbSex.AddSelectionChangedHandler("Выберите пол");
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



        // Кнопка добавления актёра в базу данных
        private void btnAddActor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполненности обязательных полей
                if (string.IsNullOrEmpty(addActorFIO.Text) ||
                    string.IsNullOrEmpty(addActorAge.Text) ||
                    cbSex.SelectedItem == null ||
                    string.IsNullOrEmpty(addActorHeight.Text) ||
                    string.IsNullOrEmpty(addActorWeight.Text) ||
                    string.IsNullOrEmpty(addActorDetails.Text) ||
                    string.IsNullOrEmpty(photoFilePath))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля и выберите фотографию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                string photoFileName = System.IO.Path.GetFileName(photoFilePath); // Сначала извлекаем имя файла
                string photoPath = System.IO.Path.Combine(imagesDirectory, photoFileName);

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

                using (var context = new TheaterEntities10())
                {
                    // Проверка, существует ли уже фотография в базе данных
                    var newPhoto = context.photo.FirstOrDefault(p => p.photo1 == photoFileName);
                    if (newPhoto == null)
                    {
                        newPhoto = new photo
                        {
                            photo1 = photoFileName,
                            description = System.IO.Path.GetFileNameWithoutExtension(photoFilePath)
                        };
                        context.photo.Add(newPhoto);
                        context.SaveChanges();
                    }

                    // Добавление нового представления с привязкой к новой фотографии
                    actors newActor = new actors
                    {
                        fio = addActorFIO.Text,
                        age = int.Parse(addActorAge.Text),
                        sex = (cbSex.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        height = int.Parse(addActorHeight.Text),
                        weight = int.Parse(addActorWeight.Text),
                        contact_details = addActorDetails.Text,
                        id_photo = newPhoto.id_photo
                    };
                    context.actors.Add(newActor);
                    context.SaveChanges();
                }

                MessageBox.Show("Актёр добавлен!");
            }
            catch (DbUpdateException dbEx)
            {
                var errorMessages = dbEx.Entries.Select(en => en.Entity.GetType().Name + " - State: " + en.State.ToString());
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = $"DbUpdateException error messages: {fullErrorMessage}";

                // Вспомогательный код для получения дополнительной информации
                if (dbEx.InnerException != null)
                {
                    exceptionMessage += $"\nInner exception: {dbEx.InnerException.Message}";
                    if (dbEx.InnerException.InnerException != null)
                    {
                        exceptionMessage += $"\nInner exception detail: {dbEx.InnerException.InnerException.Message}";
                    }
                }

                MessageBox.Show($"Ошибка при добавлении спектакля: {exceptionMessage}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении спектакля: {ex.Message}");
            }

        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageActors(objUser));
        }
    }
}
