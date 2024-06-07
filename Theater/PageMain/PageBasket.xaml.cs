using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using theater.ApplicationData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = iTextSharp.text.Image;
using Paragraph = iTextSharp.text.Paragraph;
using System.ComponentModel;
using Aspose.BarCode.Generation;
using System.Diagnostics;

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

        // Класс для отображения информации на странице PageBasket при помощи Binding
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
                    ClearBasket();

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
                PdfWriter.GetInstance(doc, new FileStream("..\\..\\Receipts\\output.pdf", FileMode.Create));

                // Открываем PDF-документ для записи
                doc.Open();

                // Создаем шрифты для PDF-документа
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font bold_font = new Font(baseFont, 25, 1);
                
                // Добавление заголовка в PDF
                Paragraph title = new Paragraph("СПИСОК ТОВАРОВ: ", bold_font);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph(" ", font));
               
                // Объявляем переменные для итоговой суммы заказа и зрителя(покупателя)
                decimal? sum = 0;
                string viewerName = null;

                List<BasketItem> basketItems = GetBasketItems();

                // Перебор элементов корзины
                foreach (var item in basketItems)
                {
                    // Получение общей стоимости экземпляров элемента корзины
                    var totalPrice = item.price * item.Quantity;

                    // Добавление информации о элементе корзины в PDF
                    doc.Add(new Paragraph("Название: " + item.title, font));
                    doc.Add(new Paragraph("Дата: " + item.date.ToString("dd.MM.yyyy HH:mm"), font));
                    doc.Add(new Paragraph("Цена: " + item.price?.ToString("F2") + " руб.", font));
                    doc.Add(new Paragraph("Количество: " + item.Quantity, font));
                    doc.Add(new Paragraph("Сумма: " + totalPrice + " руб.", font));
                    doc.Add(new Paragraph(" ", font)); 

                    // Добавление общей стоимости экземпляра элемента корзины к итоговой стоимости заказа
                    sum += totalPrice;

                    // Получение ФИО зрителя
                    if (viewerName == null)
                    {
                        viewerName = item.viewerName;
                    }
                }

                // Добавление итоговой стоимости заказа в PDF
                doc.Add(new Paragraph("Итого: " + sum?.ToString("F2") + " руб.", font));
                doc.Add(new Paragraph(" ", font));

                // Добавление ФИО зрителя в PDF
                doc.Add(new Paragraph("Покупатель: " + viewerName, font));
                doc.Add(new Paragraph(" ", font));

                BitmapImage qrCodeImage = GenerateQRCode("http://firpo.ru/Public/86");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(qrCodeImage));
                    encoder.Save(memoryStream);
                    
                    Image qrImage = Image.GetInstance(memoryStream.ToArray());
                    qrImage.Alignment = Element.ALIGN_CENTER;
                    doc.Add(qrImage);
                }
                
                MessageBox.Show("PDF сформирован!");
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
                ClearBasket();
                listBasket.ItemsSource = GetBasketItems();
            }

            // Открытие PDF файла для просмотра
            try
            {
                Process.Start(new ProcessStartInfo("..\\..\\Receipts\\output.pdf") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть PDF файл: {ex.Message}");
            }
        }

        // Генерация QR кода
        private BitmapImage GenerateQRCode(string text)
        {
            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.QR, text);
            generator.Parameters.Barcode.XDimension.Pixels = 6;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                generator.Save(memoryStream, BarCodeImageFormat.Png);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        // Метод очистки корзины от всех элементов
        private void ClearBasket()
        {
            try
            {
                using (var context = new TheaterEntities6())
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
}


