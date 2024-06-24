using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace theater
{
    internal class MethodsForView
    {


        // Проверка, заблокирован ли файл другим процессом
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                MessageBox.Show("Файл заблокирован другим процессом");
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }
    }

    public static class ComboBoxExtension
    {
        public static readonly DependencyProperty SelectedItemTextProperty =
            DependencyProperty.RegisterAttached("SelectedItemText", typeof(string), typeof(ComboBoxExtension));

        public static string GetSelectedItemText(DependencyObject obj)
        {
            return (string)obj.GetValue(SelectedItemTextProperty);
        }

        public static void SetSelectedItemText(DependencyObject obj, string value)
        {
            obj.SetValue(SelectedItemTextProperty, value);
        }

        public static void AddSelectionChangedHandler(this ComboBox comboBox, string defaultText)
        {
            SetDefaultText(comboBox, defaultText);

            comboBox.SelectionChanged += (sender, e) =>
            {
                ComboBox_SelectionChanged(sender, e, defaultText);
            };
        }

        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e, string defaultText)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                ToggleButton toggleButton = FindVisualChild<ToggleButton>(comboBox);
                if (toggleButton != null)
                {
                    toggleButton.Content = (comboBox.SelectedItem as ComboBoxItem)?.Content;
                }
            }
            else
            {
                SetDefaultText(comboBox, defaultText);
            }
        }

        private static void SetDefaultText(ComboBox comboBox, string defaultText)
        {
            if (comboBox != null)
            {
                ToggleButton toggleButton = FindVisualChild<ToggleButton>(comboBox);
                if (toggleButton != null)
                {
                    toggleButton.Content = defaultText;
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}

