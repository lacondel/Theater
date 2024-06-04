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
using theater.PageAdmin;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
            AppConnect.model0db = new TheaterEntities6();
        }

        public static class UserSessin
        {
            public static int? CurrentViewerID { get; set; }

            public static void SetCurrentViewerID(int userID)
            {
                using (var context = new TheaterEntities6())
                {
                    var viewer = context.viewer.FirstOrDefault(v => v.id_user == userID);
                    if (viewer != null)
                    {
                        CurrentViewerID = viewer.id_viewer;
                    }
                }
            }
        }
        

        // Нажатие на кнопку авторизации пользователя
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.model0db.users.FirstOrDefault(x => x.login == login.Text && x.pass == password.Password);
                
                //var actorsObj = AppConnect.model0db.actors.FirstOrDefault(x => x.id_user == userObj.id_user);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка при авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    UserSessin.SetCurrentViewerID(userObj.id_user);

                    var viewerObj = AppConnect.model0db.viewer.FirstOrDefault(x => x.id_user == userObj.id_user);

                    switch (userObj.id_user_role)
                    {
                        case 1:
                            MessageBox.Show("Здравствуйте, Администратор " + userObj.login + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new PageMenuAdmin());
                            break;
                        case 2:
                            MessageBox.Show("Здравствуйте, уважаемый " + viewerObj.fio + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new ViewerNavigation());
                            break;
                        case 3:
                            //MessageBox.Show("Приветствуем вас, уважаемый " + actorsObj.fio + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new ViewerNavigation());
                            break;
                        default:
                            MessageBox.Show("Мы вас не нашли!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
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
