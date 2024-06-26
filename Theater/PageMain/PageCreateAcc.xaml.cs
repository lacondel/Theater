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
    /// Логика взаимодействия для PageCreateAcc.xaml
    /// </summary>
    public partial class PageCreateAcc : Page
    {
        public PageCreateAcc()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.model0db.users.Count(x => x.login==regLogin.Text)>0)
            {
                MessageBox.Show("Пользователь с таким логином существует!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                users usersObj = new users()
                {
                    login = regLogin.Text,
                    pass = regPass.Password,
                    id_user_role = 2
                };
                viewer viewerObj = new viewer()
                {
                    fio = regName.Text,
                    contact_details = contacts.Text,
                    id_user = usersObj.id_user
                };
                AppConnect.model0db.users.Add(usersObj);
                AppConnect.model0db.viewer.Add(viewerObj);
                AppConnect.model0db.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных!\n" + ex, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void rpChanged(object sender, RoutedEventArgs e)
        {
            Password_Match(sender, e);
        }
        private void rprChanged(object sender, RoutedEventArgs e)
        {
            Password_Match(sender, e);
        }

        private void Password_Match(object sender, RoutedEventArgs e)
        {
            if (regPass.Password!=regPassRepeat.Password)
            {
                btnCreate.IsEnabled = false;
                regPassRepeat.Background = Brushes.LightCoral;
                regPassRepeat.BorderBrush = Brushes.Red;
            }
            else
            {
                btnCreate.IsEnabled = true;
                regPassRepeat.Background = Brushes.LightGreen;
                regPassRepeat.BorderBrush = Brushes.Green;
            }
        }
    }
}
