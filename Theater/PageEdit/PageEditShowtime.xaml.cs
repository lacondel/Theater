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
using theater.PageMain;

namespace theater.PageEdit
{
    /// <summary>
    /// Логика взаимодействия для PageEditShowtime.xaml
    /// </summary>
    public partial class PageEditShowtime : Page
    {
        // Переменная для хранения данных текущего объекта представление
        private showtime currentShowtime;

        public users objUser { get; set; }

        public PageEditShowtime(showtime showtime, users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
            currentShowtime = showtime;
            LoadShowtimeData();
        }



        // Подгружаем данные о выбранном представлении
        private void LoadShowtimeData()
        {
            editShowtimeDate.Text = currentShowtime.date.ToString();
            editShowtimePrice.Text = currentShowtime.price.ToString();
        }



        // Кнопка сохранения изменений 
        private void btnEditShowtime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new TheaterEntities10())
                {
                    var showtimeToUpdate = context.showtime.FirstOrDefault(s => s.id_showtime == currentShowtime.id_showtime);
                    if (showtimeToUpdate != null)
                    {
                        showtimeToUpdate.date = DateTime.Parse(editShowtimeDate.Text);
                        showtimeToUpdate.price = Decimal.Parse(editShowtimePrice.Text);

                        context.SaveChanges();
                    }
                }
                // Уведомляем о успешном сохранении изменений и переходим на страницу PageListOfPerformance
                MessageBox.Show("Изменения успешно сохранены.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.Navigate(new PageShowtimes(objUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes(objUser));
        }
    }
}
