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
                string imagePath = "/Images/" + str;

                // Проверка существования файла
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath.TrimStart('/'))))
                {
                    return imagePath;
                }
                else
                {
                    return "/Images/picture.png";
                }
            }
        }
    }
}
