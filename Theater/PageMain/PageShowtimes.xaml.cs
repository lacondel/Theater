using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using theater.ApplicationData;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageShowtimes.xaml
    /// </summary>
    public partial class PageShowtimes : Page
    {
        public PageShowtimes()
        {
            InitializeComponent();
            listOfShowtimes.ItemsSource = FindShowtime();
        }

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

                            showtimes = showtimes.Where(x => x.performance.genre == "драма").ToList();
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

                if (showtimes.Count > 0)
                {
                    tbCounter.Text = "Найдено " + showtimes.Count + " показов.";
                }
                else
                {
                    tbCounter.Text = "Показы не найдены.";
                }

                return showtimes.ToArray();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                return null;
            }
        }

        private void searchChanged(object sender, TextChangedEventArgs e)
        {
            listOfShowtimes.ItemsSource = FindShowtime();
        }

        private void sortChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfShowtimes.ItemsSource = FindShowtime();
        }

        private void filterChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfShowtimes.ItemsSource = FindShowtime();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new ViewerNavigation());
        }

        private void btnAddToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (listOfShowtimes.SelectedItem != null)
            {
                performance selectedPerformance = (performance)listOfShowtimes.SelectedItem;

                try
                {
                    //using (var context = new SportClubEntities1())
                    //{
                    //    Basket basketItem = new Basket
                    //    {
                    //        idAbon = selectedAbonement.idAbon,
                    //        idClient = PageLogin.id_viewer
                    //    };

                    //    context.Basket.Add(basketItem);
                    //    context.SaveChanges();
                    //}

                    MessageBox.Show("Абонемент успешно добавлен в корзину!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении абонемента в корзину: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите абонемент для добавления в корзину!");
            }
        }

        private void btnGoToBasket_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageBasket());
        }


        public class ShowtimeViewModel
        {
            public string Title { get; set; }
            public DateTime Date { get; set; }
            public decimal Price { get; set; }

        }
    }
}

