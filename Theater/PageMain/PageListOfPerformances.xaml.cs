using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using theater.ApplicationData;
using theater.PageAdmin;
using theater.PageEdit;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageListOfPerformances.xaml
    /// </summary>
    public partial class PageListOfPerformances : Page
    {
        public users objUser {  get; set; }

        public PageListOfPerformances(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            listOfPerformances.ItemsSource = FindPerformance();
            RenderBtn(this.objUser);
            // Добавляем обработчик события SelectionChanged для ComboBox sortPerform
            sortPerform.AddSelectionChangedHandler("Сортировка");
            // Добавляем обработчик события SelectionChanged для ComboBox filterPerform
            filterPerform.AddSelectionChangedHandler("Фильтрация");
        }


        // Отображение кнопок администратора
        public void RenderBtn(users objUser) 
        {
            if (objUser.id_user_role == 1)
            {
                btnAddPerformance.Visibility = Visibility.Visible;
                btnDeletePerformance.Visibility = Visibility.Visible;
                btnEditPerformance.Visibility = Visibility.Visible;
            }
        }



        // Сортировка, поиск, фильтрация функционал
        performance[] FindPerformance()
        {
            int sortIndex = sortPerform.SelectedIndex;
            int filterIndex = filterPerform.SelectedIndex;
            try
            {
                List<performance> performances = AppConnect.model0db.performance.ToList();

                if (!string.IsNullOrEmpty(search?.Text))
                {
                    performances = performances.Where(x => x.title.ToLowerInvariant().Contains(search.Text.ToLowerInvariant())).ToList();
                }

                if (filterIndex > 0)
                {
                    switch (filterIndex)
                    {
                        case 1:
                            performances = performances.Where(x => x.genre == "Драма").ToList();
                            break;
                        case 2:
                            performances = performances.Where(x => x.genre == "Водевиль").ToList();
                            break;
                        case 3:
                            performances = performances.Where(x => x.genre == "Мюзикл").ToList();
                            break;
                        case 4:
                            performances = performances.Where(x => x.genre == "Комедия").ToList();
                            break;
                    }
                }

                if (sortIndex > 0)
                {
                    switch (sortIndex)
                    {
                        case 1:
                            performances = performances.OrderBy(x => x.title).ToList();
                            break;
                        case 2:
                            performances = performances.OrderByDescending(x => x.title).ToList();
                            break;
                        case 3:
                            performances = performances.OrderBy(x => x.year_created).ToList();
                            break;
                        case 4:
                            performances = performances.OrderByDescending(x => x.year_created).ToList();
                            break;
                    }
                }

                int lastDigit = performances.Count % 10;
                int lastTwoDigits = performances.Count % 100;

                if (performances.Count == 0)
                {
                    tbCounter.Text = "Спектакли не найдены";
                }
                else if (lastDigit == 1 && lastTwoDigits != 11)
                {
                    tbCounter.Text = "Найден " + performances.Count + " спектакль";
                }
                else if ((lastDigit == 2 || lastDigit == 3 || lastDigit == 4) && (lastTwoDigits != 12 && lastTwoDigits != 13 && lastTwoDigits != 14))
                {
                    tbCounter.Text = "Найдено " + performances.Count + " спектакля";
                }
                else
                {
                    tbCounter.Text = "Найдено " + performances.Count + " спектаклей";
                }

                return performances.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                return null;
            }
        }



        // Поле поиска
        private void searchChanged(object sender, TextChangedEventArgs e)
        {
            listOfPerformances.ItemsSource = FindPerformance();
        }



        // Кнопка сортировки
        private void sortChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfPerformances.ItemsSource = FindPerformance();
        }



        // Кнопка фильтрации
        private void filterChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfPerformances.ItemsSource = FindPerformance();
        }



        // Кнопка для перехода к форме добавления спектакля
        private void btnAddPerformance_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddPerformance(objUser));
        }



        // Конопка удаления выбранного спектакля
        private void btnDeletePerformance_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, выбран ли спектакль для удаления
            if (listOfPerformances.SelectedItem == null)
            {
                MessageBox.Show("Выберите спектакль для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);   
            }
            
            // Преобразуем выбранный элемент в объект типа "performance", если преобразование не удалось, выводим ошибку
            var selectedPerformance = listOfPerformances.SelectedItem as performance;
            if (selectedPerformance == null)
            {
                MessageBox.Show("Ошибка при получении данных о спектакле.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Переменная, хранящая ответ выбранный в MessageBox
            var result = MessageBox.Show($"Вы уверены, что хотите удалить спектакль '{selectedPerformance.title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TheaterEntities10())
                    {
                        var performanceToDelete = context.performance.FirstOrDefault(p => p.id_performance == selectedPerformance.id_performance);
                        if (performanceToDelete != null) 
                        { 
                            context.performance.Remove(performanceToDelete);
                            context.SaveChanges();
                        }
                    }
                    listOfPerformances.ItemsSource = FindPerformance();
                    MessageBox.Show("Спектакль успешно удален.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Ошибка при удалении спектакля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);  
                }
            }
        }



        // Кнопка для перехода к форме редактирования выбранного спектакля
        private void btnEditPerformance_Click(object sender, EventArgs e)
        {
            // Проверка, выбран ли спектакль для редактирования
            if (listOfPerformances.SelectedItem == null) 
            {
                MessageBox.Show("Выберите спектакль для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Преобразование выбранного элемента в объект типа "performance", если преобразование не проходит, выводим ошибку
            var selectedPerformance = listOfPerformances.SelectedItem as performance;
            if (selectedPerformance == null)
            {
                MessageBox.Show("Ошибка при получении данных о спектакле.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Переходим на страницу редактирования спектакля
            AppFrame.frameMain.Navigate(new PageEditPerformance(selectedPerformance, objUser));
        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new ViewerNavigation(objUser));
        }
    }
}
