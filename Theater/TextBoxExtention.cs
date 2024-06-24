using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace theater
{
    public static class TextBoxExtensions
    {
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.RegisterAttached(
                "Hint", typeof(string), typeof(TextBoxExtensions),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.None, HintPropertyChanged));

        public static string GetHint(DependencyObject obj)
        {
            return (string)obj.GetValue(HintProperty);
        }

        public static void SetHint(DependencyObject obj, string value)
        {
            obj.SetValue(HintProperty, value);
        }

        private static void HintPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.Loaded += TextBox_Loaded;
                textBox.GotFocus += TextBox_GotFocus;
                textBox.LostFocus += TextBox_LostFocus;
                ApplyHint(textBox);
            }
        }

        private static void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyHint(sender as TextBox);
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == GetHint(textBox))
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black; // Assuming you want black text when focused
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplyHint(sender as TextBox);
        }

        private static void ApplyHint(TextBox textBox)
        {
            if (textBox != null)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = GetHint(textBox);
                    textBox.Foreground = Brushes.Gray; // Gray color for hint
                }
                else
                {
                    textBox.Foreground = Brushes.Black; // Black color for regular text
                }
            }
        }
    }
}
