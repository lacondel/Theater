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
using System.Xml.Linq;
using theater.ApplicationData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = iTextSharp.text.Image;
using System.Windows.Markup;
using Paragraph = iTextSharp.text.Paragraph;

namespace theater.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageBasket.xaml
    /// </summary>
    public partial class PageBasket : Page
    {
        public PageBasket()
        {
            InitializeComponent();
        }

        //private void btnPay_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes());
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreatePDF()
        {
            Document doc = new Document();

            try
            {
                PdfWriter.GetInstance(doc, new FileStream("*..\\..\\output.pdf", FileMode.Create));

                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font font1 = new Font(baseFont, 25, 3, BaseColor.BLUE);
                Paragraph paragraph1 = new Paragraph("СПИСОК ТОВАРОВ: " + font1);
                paragraph1.Alignment = Element.ALIGN_CENTER;
                doc.Add(paragraph1);
                decimal? sum = 0;

                foreach (var item in AppConnect.model0db.performance.ToList())
                {
                    if (item is performance)
                    {
                        performance data = (performance)item;

                        doc.Add(new Paragraph ("Название: " + data.title, font));
                        doc.Add(new Paragraph("Цена: " + data.price, font));

                        sum += data.price;
                    }
                }
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }
            finally
            {
                doc.Close();
            }
        }
    }
}
