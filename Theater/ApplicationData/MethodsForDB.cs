using System.Linq;
using System.Windows.Controls;
using System.Windows;


namespace theater.ApplicationData
{


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
            var performance = AppConnect.model0db.performance.FirstOrDefault(p => p.id_performance == id);

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
