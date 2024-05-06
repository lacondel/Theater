using System;
using System.Collections.Generic;
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
            listOfPerformances.ItemsSource = FindPerformance();
        }

        performance[] FindPerformance()
        {
            try
            {
                List<performance> performances = AppConnect.model0db.performance.ToList();
                if (search != null)
                {
                    performances = performances.Where(x => x.title.ToLower().Contains(search.Text.ToLower())).ToList();
                }

                if (filterPerform.SelectedIndex > 0)
                {
                    switch (filterPerform.SelectedIndex)
                    {
                        case 0:
                            performances = (List<performance>)performances.Where(x => x.genre == "драма");
                            break;
                    }
                }

                if (sortPerform.SelectedIndex >= 0)
                {
                    switch (sortPerform.SelectedIndex)
                    {
                        case 0:
                            performances = performances.OrderBy(x => x.title).ToList();
                            break;
                        case 1:
                            performances = performances.OrderByDescending(x => x.title).ToList();
                            break;
                        case 2:
                            performances = performances.OrderBy(x => x.year_created).ToList();
                            break;
                        case 3:
                            performances = performances.OrderByDescending(x => x.year_created).ToList();
                            break;
                    }
                }

                if (performances.Count > 0)
                {
                    tbCounter.Text = "Найдено " + performances.Count + " спектаклей.";
                }
                else
                {
                    tbCounter.Text = "Спектакли не найдены.";
                }

                return performances.ToArray();
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
                return null;
            }
        }

        private void searchChanged(object sender, TextChangedEventArgs e)
        {
            listOfPerformances.ItemsSource = FindPerformance();
        }

        private void sortChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfPerformances.ItemsSource = FindPerformance();
        }

        private void filterChanged(object sender, SelectionChangedEventArgs e)
        {
            listOfPerformances.ItemsSource = FindPerformance();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new ViewerNavigation());
        }

        private void btnAddToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (listOfPerformances.SelectedItem != null)
            {
                performance selectedPerformance = (performance)listOfPerformances.SelectedItem;

                //try
                //{
                //    using (var context = new SportClubEntities1())
                //    {
                //        Basket basketItem = new Basket
                //        {
                //            idAbon = selectedAbonement.idAbon,
                //            idClient = PageLogin.currClientId
                //        };

                //        context.Basket.Add(basketItem);
                //        context.SaveChanges();
                //    }

                //    MessageBox.Show("Абонемент успешно добавлен в корзину!");
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show($"Ошибка при добавлении абонемента в корзину: {ex.Message}");
                //}
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
    }
}

