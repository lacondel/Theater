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
using System.ComponentModel;
using System.Security.RightsManagement;

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
            listBasket.ItemsSource = GetBasketItems();
        }

        public class BasketItem : INotifyPropertyChanged
        {
            private int quantity;

            public int id { get; set; }
            public string title { get; set; }
            public DateTime date { get; set; }
            public decimal? price { get; set; }
            public string viewerName { get; set; }
            public int Quantity
            {
                get => quantity;
                set
                {
                    if (quantity != value)
                    {
                        quantity = value;
                        OnPropertyChanged(nameof(Quantity));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private List<BasketItem> GetBasketItems()
        {
            try
            {
                using (var context = new TheaterEntities6())
                {
                    var basketItems = context.basket
                        .Include("showtime.performance")
                        .Include("viewer")
                        .Select(b => new BasketItem
                        {
                            id = b.id_basket,
                            title = b.showtime.performance.title,
                            date = b.showtime.date,
                            price = b.showtime.price,
                            viewerName = b.viewer.fio,
                            Quantity = b.quantity
                        })
                        .ToList();

                    return basketItems;
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show($"Ошибка при извлечении данных из корзины: {ex.Message}");
                return new List<BasketItem>();
            }
        }

        // Отработка клика кнопки "+" добавленного показа
        private void btnIncreaseQuantity_Click( object sender, EventArgs e )
        {
            var button = sender as Button;
            var basketItem = button?.CommandParameter as BasketItem;

            if (basketItem != null )
            {
                try
                {
                    using ( var context = new TheaterEntities6())
                    {
                        var itemToUpdate = context.basket.FirstOrDefault(b => b.id_basket == basketItem.id);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.quantity++;
                            context.SaveChanges();

                            basketItem.Quantity++;
                        }
                    }
                }
                catch (Exception ex )
                {
                    MessageBox.Show($"Ошибка при увеличении количества: {ex.Message}");
                }
            }
        }

        // Отработка клика кнопки "-" добавленного показа
        private void btnDecreaseQuantity_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var basketItem = button?.CommandParameter as BasketItem;

            if (basketItem != null && basketItem.Quantity > 0)
            {
                try
                {
                    using (var context = new TheaterEntities6())
                    {
                        var itemToUpdate = context.basket.FirstOrDefault(b => b.id_basket == basketItem.id);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.quantity--;
                            context.SaveChanges();

                            basketItem.Quantity--;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при уменьшении количества: {ex.Message}");
                }
            }
        }

        // Отработка клика кнопки "Удалить" добавленного показа
        private void btnDeleteFromBasket_Click (object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var basketItem = button?.CommandParameter as BasketItem;
            if (basketItem != null)
            {
                try
                {
                    using (var context = new TheaterEntities6())
                    {
                        var itemToRemove = context.basket.FirstOrDefault(b => b.id_basket == basketItem.id);
                        if (itemToRemove != null)
                        {
                            context.basket.Remove(itemToRemove);
                            context.SaveChanges();

                            MessageBox.Show("Элемент успешно удален из корзины!");

                            // Обновляем отображение списка корзины
                            listBasket.ItemsSource = GetBasketItems();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении элемента: {ex.Message}");
                }
            }
        }

        // Отработка клика кнопки "Вернуться"
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageShowtimes());
        }

        // Отработка клика кнопки "Оплатить заказ"
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            CreatePDF();
        }

        // Отработка клика кнопки "Очистить корзину"
        private void btnClearBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new TheaterEntities6())
                {
                    var basketItems = context.basket.ToList();
                    context.basket.RemoveRange(basketItems);
                    context.SaveChanges();

                    MessageBox.Show("Корзина успешно очищена!");

                    // Обновляем отображение списка корзины
                    listBasket.ItemsSource = GetBasketItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при очисте корзины: {ex.Message}");
            }
        }

        private void CreatePDF()
        {
            Document doc = new Document();

            try
            {
                PdfWriter.GetInstance(doc, new FileStream("*..\\..\\output.pdf", FileMode.Create));

                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font bold_font = new Font(baseFont, 25, 1);
                
                Paragraph paragraph1 = new Paragraph("СПИСОК ТОВАРОВ: " + bold_font);
                paragraph1.Alignment = Element.ALIGN_CENTER;
                doc.Add(paragraph1);
               
                decimal? sum = 0;

                List<BasketItem> basketItems = GetBasketItems();

                foreach (var item in basketItems)
                {
                    var totalPrice = item.price * item.Quantity;
                    doc.Add(new Paragraph("Название: " + item.title, font));
                    doc.Add(new Paragraph("Дата: " + item.date.ToString("dd.MM.yyyy HH:mm"), font));
                    doc.Add(new Paragraph("Цена: " + item.price?.ToString("F2") + " руб.", font));
                    doc.Add(new Paragraph("Количество: " + item.Quantity, font));
                    doc.Add(new Paragraph("Сумма: " + totalPrice + " руб.", font));
                    doc.Add(new Paragraph(" ", font)); 

                    sum += totalPrice;
                }

                doc.Add(new Paragraph("Итого: " + sum, font));
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
