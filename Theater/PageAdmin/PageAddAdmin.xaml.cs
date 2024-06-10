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

namespace theater.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddAdmin.xaml
    /// </summary>
    public partial class PageAddAdmin : Page
    {
        public PageAddAdmin()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageMenuAdmin());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new TheaterEntities6())
                {
                    // Создаем новый объект пользователя
                    users newUser = new users
                    {
                        login = addAdminLogin.Text,
                        pass = addAdminPassword.Text,
                        id_user_role = 1
                    };

                    // Добавляем нового пользователя в контекст
                    context.users.Add(newUser);

                    //Сохраняем изменения в базе данных
                    context.SaveChanges();

                    MessageBox.Show("Администратор успешно добавллени!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении администратора: {ex.Message}");
            }
        }
    }
}
