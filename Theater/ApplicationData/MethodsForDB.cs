using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace theater.ApplicationData
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ClearBasket();
        }

        private void ClearBasket()
        {
            try
            {
                using (var context = new TheaterEntities5())
                {
                    var basketItems = context.basket.ToList();
                    context.basket.RemoveRange(basketItems);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при очистке корзины: {ex.Message}");
            }
        }
    }

    public static class MethodsForDB
    {
        public static string PhotoPath(int id)
        {
            var ph = AppConnect.model0db.photo.FirstOrDefault(p => p.id_photo == id);

            if (ph != null)
            {
                return "/Images/" + ph.photo1;
            }
            else
            {
                return "/Images/picture.png";
            }
        }

        public static string Title(int id)
        {
            var performance = AppConnect.model0db.performance.FirstOrDefault(p => p.id_performans == id);

            if (performance != null)
            {
                return performance.title;
            }
            else
            {
                return "Спектакль не найден";
            }
            
        }
    }
}
