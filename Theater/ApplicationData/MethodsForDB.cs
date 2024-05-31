using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace theater.ApplicationData
{
    public static class MethodsForDB
    {
        public static string PhotoPath(String str)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str))
            {
                return "/Images/picture.png";
            }
            else
            {
                return "/Images/" + str;
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
