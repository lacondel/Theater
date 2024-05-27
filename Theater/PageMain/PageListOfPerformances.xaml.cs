using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
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
    /// Логика взаимодействия для PageListOfPerformances.xaml
    /// </summary>
    public partial class PageListOfPerformances : Page
    {
        public PageListOfPerformances()
        {
            InitializeComponent();
            listOfPerformances.ItemsSource = FindPerformance();
        }

        performance[] FindPerformance()
        {
            int sortIndex = sortPerform.SelectedIndex;
            int filterIndex = filterPerform.SelectedIndex;
            try
            {
                List<performance> performances = AppConnect.model0db.performance.ToList();
                if (search != null)
                {
                    performances = performances.Where(x => x.title.ToLower().Contains(search.Text.ToLower())).ToList();
                }

                if (filterIndex > 0)
                {
                    switch (filterIndex)
                    {
                        case 0:
                            performances = (List<performance>)performances.Where(x => x.genre == "драма");
                            break;
                    }
                }

                if (sortIndex > 0)
                {
                    switch (sortIndex)
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
    }


}
