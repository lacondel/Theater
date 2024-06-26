using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using theater.ApplicationData;
using theater.PageAdmin;
using theater.PageEdit;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageShowtimes.xaml
    /// </summary>
    public partial class PageShowtimes : Page
    {
        public users objUser { get; set; }

        public PageShowtimes(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            listOfShowtimes.ItemsSource = FindShowtime();
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
                btnAddShowtime.Visibility = Visibility.Visible;
                btnDeleteShowtime.Visibility = Visibility.Visible;
                btnEditShowtime.Visibility = Visibility.Visible;
            }
        }



        // Сортировка, поиск, фильтрация функционал
        public showtime[] FindShowtime()
        {
            try
            {
                List<showtime> showtimes = AppConnect.model0db.showtime.Include("performance").ToList();

                if (!string.IsNullOrEmpty(search?.Text))
                {
                    showtimes = showtimes.Where(x => x.performance.title.ToLower().Contains(search.Text.ToLower())).ToList();
                }

                if (filterPerform.SelectedIndex > 0)
                {
                    switch (filterPerform.SelectedIndex)
                    {
                        case 1:
                            showtimes = showtimes.Where(x => x.PerformanceGenre == "Драма").ToList();
                            break;
                        case 2:
                            showtimes = showtimes.Where(x => x.PerformanceGenre == "Водевиль").ToList();
                            break;
                        case 3:
                            showtimes = showtimes.Where(x => x.PerformanceGenre == "Мюзикл").ToList();
                            break;
                        case 4:
                            showtimes = showtimes.Where(x => x.PerformanceGenre == "Комедия").ToList();
                            break;
                    }
                }

                if (sortPerform.SelectedIndex > 0)
                {
                    switch (sortPerform.SelectedIndex)
                    {
                        case 1:
                            showtimes = showtimes.OrderBy(x => x.performance.title).ToList();
                            break;
                        case 2:
                            showtimes = showtimes.OrderByDescending(x => x.performance.title).ToList();
                            break;
                        case 3:
                            showtimes = showtimes.OrderBy(x => x.performance.year_created).ToList();
                            break;
                        case 4:
                            showtimes = showtimes.OrderByDescending(x => x.performance.year_created).ToList();
                            break;
                    }
                }

                int lastDigit = showtimes.Count % 10;
                int lastTwoDigits = showtimes.Count % 100;

                if (showtimes.Count == 0)
                {
                    tbCounter.Text = "Представления не найдены";
                }
                else if (lastDigit == 1 && lastTwoDigits != 11)
                {
                    tbCounter.Text = "Найдено " + showtimes.Count + " представление";
                }
                else if ((lastDigit == 2 || lastDigit == 3 || lastDigit == 4) && (lastTwoDigits != 12 && lastTwoDigits != 13 && lastTwoDigits != 14))
                {
                    tbCounter.Text = "Найдено " + showtimes.Count + " представления";
                }
                else
                {
                    tbCounter.Text = "Найдено " + showtimes.Count + " представлений";
                }

                return showtimes.ToArray();
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
            listOfShowtimes.ItemsSource = FindShowtime();
        }



        // Кнопка сортировки
        private void sortChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfShowtimes.ItemsSource = FindShowtime();
        }



        // Кнопка фильтрации
        private void filterChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfShowtimes.ItemsSource = FindShowtime();
        }


        // Кнопка для перехода к форме добавления представления
        private void btnAddShowtime_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddShowtime(objUser));
        }



        // Конпка удаления выбранного представления
        private void btnDeleteShowtime_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, выбрано ли представление для удаления
            if (listOfShowtimes.SelectedItem == null)
            {
                MessageBox.Show("Выберите представление для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Преобразуем выбранный элемент в объект типа "showtime", если преобразование не удалось, выводим ошибку
            var selectedShowtime = listOfShowtimes.SelectedItem as showtime;
            if (selectedShowtime == null)
            {
                MessageBox.Show("Ошибка при получении данных о представлении.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Переменная, хранящая ответ выбранный в MessageBox
            var result = MessageBox.Show($"Вы уверены, что хотите удалить представление '{selectedShowtime.performance.title}' {selectedShowtime.date}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new TheaterEntities10())
                    {
                        var showtimeToDelete = context.showtime.FirstOrDefault(p => p.id_showtime == selectedShowtime.id_showtime);
                        if (showtimeToDelete != null)
                        {
                            context.showtime.Remove(showtimeToDelete);
                            context.SaveChanges();
                        }
                    }
                    listOfShowtimes.ItemsSource = FindShowtime();
                    MessageBox.Show("Представление успешно удалено.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении представления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        // Кнопка для перехода к форме редактирования выбранного представления
        private void btnEditShowtime_Click(object sender, EventArgs e)
        {
            // Проверка, выбрано ли представление для редактирования
            if (listOfShowtimes.SelectedItem == null)
            {
                MessageBox.Show("Выберите представление для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Преобразование выбранного элемента в объект типа "showtime", если преобразование не проходит, выводим ошибку
            var selectedShowtime = listOfShowtimes.SelectedItem as showtime;
            if (selectedShowtime == null)
            {
                MessageBox.Show("Ошибка при получении данных о представлении.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Переходим на страницу редактирования спектакля
            AppFrame.frameMain.Navigate(new PageEditShowtime(selectedShowtime, objUser));
        }

        // Кнопка добавления представления в корзину
        private void btnAddToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (listOfShowtimes.SelectedItem != null)
            {
                showtime addbleShowtime = (showtime)listOfShowtimes.SelectedItem;

                try
                {
                    using (var context = new TheaterEntities10())
                    {
                        var existingItem = context.basket.FirstOrDefault(b => b.id_showtime == addbleShowtime.id_showtime);

                        if (existingItem != null)
                        {
                            existingItem.quantity++;
                        }
                        else
                        {
                            basket basketItem = new basket
                            {
                                id_viewer = AppConnect.model0db.viewer.FirstOrDefault(x => x.id_user == objUser.id_user).id_viewer,
                                id_showtime = addbleShowtime.id_showtime,
                                quantity = 1
                            };

                            context.basket.Add(basketItem);
                        }

                        context.SaveChanges();
                    }

                    MessageBox.Show("Представление успешно добавлено в корзину!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении показа в корзину:\n {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите показ для добавления его в корзину!");
            }
        }



        // Кнопка перехода к корзине
        private void btnGoToBasket_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageBasket(objUser));
        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new ViewerNavigation(objUser));
        }
    }
}

