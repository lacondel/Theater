using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using theater.ApplicationData;
using theater.PageAdmin;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public static int? CurrentViewerID { get; set; }
        public static int? CurrentUserRoleID { get; set; }

        public PageLogin()
        {
            InitializeComponent();
            AppConnect.model0db = new TheaterEntities10();
        }

        // Нажатие на кнопку авторизации пользователя
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.model0db.users.Include("viewer").FirstOrDefault(x => x.login == login.Text && x.pass == password.Password);

                
                if (userObj == null)
                {
                    MessageBox.Show("Допущена ошибка в логине или пароле!", "Ошибка при авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var viewerObj = AppConnect.model0db.viewer.FirstOrDefault(x => x.id_user == userObj.id_user);

                    switch (userObj.id_user_role)
                    {
                        case 1:
                            MessageBox.Show("Здравствуйте, администратор " + userObj.login + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new ViewerNavigation(userObj));
                            break;
                        case 2:
                            MessageBox.Show("Здравствуйте, уважаемый " + viewerObj.fio + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new ViewerNavigation(userObj));
                            break;
                        default:
                            MessageBox.Show("Допущеная ошибка в логине или пароле!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageCreateAcc());
        }
    }
}
