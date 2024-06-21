using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public users objUser { get; set; }

        public PageShowtimes(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            listOfShowtimes.ItemsSource = FindShowtime();
            RenderBtn(this.objUser);
        }

        public void RenderBtn(users objUser)
        {
            if (objUser.id_user_role == 1)
            {
                btnAddShowtime.Visibility = Visibility.Visible;
                btnDeleteShowtime.Visibility = Visibility.Visible;
                btnEditShowtime.Visibility = Visibility.Visible;
            }
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
            AppFrame.frameMain.Navigate(new ViewerNavigation(objUser));
        }

        private void btnAddToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (listOfShowtimes.SelectedItem != null)
            {
                showtime selectedShowtime = (showtime)listOfShowtimes.SelectedItem;

                try
                {
                    using (var context = new TheaterEntities7())
                    {
                        var existingItem = context.basket.FirstOrDefault(b => b.id_showtime == selectedShowtime.id_showtime);

                        if (existingItem != null)
                        {
                            existingItem.quantity++;
                        }
                        else
                        {
                            basket basketItem = new basket
                            {
                                id_viewer = AppConnect.model0db.viewer.FirstOrDefault(x => x.id_user == objUser.id_user).id_viewer,
                                id_showtime = selectedShowtime.id_showtime,
                                quantity = 1
                            };

                            context.basket.Add(basketItem);
                        }
                        
                        context.SaveChanges();
                    }

                    MessageBox.Show("Показ успешно добавлен в корзину!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении показа в корзину: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите показ для добавления его в корзину!");
            }
        }

        private void btnGoToBasket_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageBasket(objUser));
        }

    }
}

