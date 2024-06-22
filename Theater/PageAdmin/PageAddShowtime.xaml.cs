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

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddShowtime.xaml
    /// </summary>
    public partial class PageAddShowtime : Page
    {
        public users objUser { get; set; }

        // Переменная для пути к изображению на компьютере
        private string photoFilePath;

        public PageAddShowtime(users objUser)
        {
            this.objUser = objUser;
            InitializeComponent();
        }



        // Конопка добавления представления в базу данных
        private void btnAddShowtime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Добавление данных в базу данных
                using (var context = new TheaterEntities10())
                {
                    // Добавление представления в базу данных
                    showtime newShowtime = new showtime
                    {
                        id_performanсe = context.performance.FirstOrDefault(p => p.title == addPerformanceTitle.Text).id_performance,
                        date = DateTime.Parse(addShowtimeDate.Text),
                        price = decimal.Parse(addShowtimePrice.Text),
                        id_photo = context.performance.FirstOrDefault(p => p.title == addPerformanceTitle.Text).id_photo
                    };
                    context.showtime.Add(newShowtime);
                    context.SaveChanges();
                }

                MessageBox.Show("Представление добавлено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении представления: {ex.Message}");
            }
        }



        // Кнопка возвращения на предыдущую страницу
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes(objUser));
        }
    }
}
