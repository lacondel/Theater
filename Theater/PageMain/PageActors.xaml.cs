using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using theater.ApplicationData;
using theater.PageAdmin;
using theater.PageEdit;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageActors.xaml
    /// </summary>
    public partial class PageActors : Page
    {
        public users objUser { get; set; }

        public PageActors(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            listOfActors.ItemsSource = AppConnect.model0db.actors.ToList();
            RenderBtn(this.objUser);
        }



        // Отображение кнопок администратора
        public void RenderBtn(users objUser)
        {
            if (objUser.id_user_role == 1)
            {
                btnAddActor.Visibility = Visibility.Visible;
                btnDeleteActor.Visibility = Visibility.Visible;
                btnEditActor.Visibility = Visibility.Visible;
            }
        }



        // Кнопка для перехода к форме добавления актёра
        private void btnAddActor_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddActor(objUser));
        }



        // Кнопка удаления выбранного актёра
        private void btnDeleteActor_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, выбран ли актёр для удаления
            if (listOfActors.SelectedItem == null)
            {
                MessageBox.Show("Выберите актёра для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Преобразуем выбранный элемент в объект типа "actors", если преобразование не удалось, выводим ошибку
            var selectedActor = listOfActors.SelectedItem as actors;
            if (selectedActor == null)
            {
                MessageBox.Show("Ошибка при получении данных о актёре.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Переменная, хранящая ответ выбранный в MessageBox
            var result = MessageBox.Show($"Вы уверены, что хотите удалить актёра '{selectedActor.fio}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TheaterEntities10())
                    {
                        var actorToDelete = context.actors.FirstOrDefault(p => p.id_actor == selectedActor.id_actor);
                        if (actorToDelete != null)
                        {
                            context.actors.Remove(actorToDelete);
                            context.SaveChanges();
                        }
                    }
                    listOfActors.ItemsSource = AppConnect.model0db.actors.ToList();
                    MessageBox.Show("Актёр успешно удален.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении актёра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        // Кнопка для перехода к форме редактирования выбранного актёра
        private void btnEditActor_Click(object sender, EventArgs e)
        {
            // Проверка, выбран ли спектакль для редактирования
            if (listOfActors.SelectedItem == null)
            {
                MessageBox.Show("Выберите спектакль для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Преобразование выбранного элемента в объект типа "performance", если преобразование не проходит, выводим ошибку
            var selectedActor = listOfActors.SelectedItem as actors;
            if (selectedActor== null)
            {
                MessageBox.Show("Ошибка при получении данных о спектакле.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Переходим на страницу редактирования спектакля
            AppFrame.frameMain.Navigate(new PageEditActor(selectedActor, objUser));
        }


        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new ViewerNavigation(objUser));
        }
    }
}
