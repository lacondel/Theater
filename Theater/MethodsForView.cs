using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace theater
{
    internal class MethodsForView
    {
        // Подсказка для ComboBox'ов
        public static void InitializeComboBox(ComboBox comboBox)
        {
            // Утсанавливаем элемент-заполнитель при загрузке
            if (comboBox.Items.Count > 0 && comboBox.Items[0] is ComboBoxItem placeholderItem && !placeholderItem.IsEnabled)
            {
                comboBox.SelectedIndex = 0;
            }

            // Удаляем элемент-заполнитель при изменении выбора
            comboBox.SelectionChanged += (sender, e) =>
            {
                if (comboBox != null)
                {
                    if (comboBox.SelectedIndex != 0 && comboBox.Items[0] is ComboBoxItem placeholder && !placeholder.IsEnabled)
                    {
                        comboBox.Items.RemoveAt(0);
                    }
                }
            };
        }
    }
}
