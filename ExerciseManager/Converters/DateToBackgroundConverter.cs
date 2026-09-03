using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ExerciseManager.Converters
{
    public class DateToBackgroundConverter: IValueConverter
    {
        // Lista dat, które chcemy wyróżnić
        public static List<DateTime> HighlightedDates { get; set; } = new List<DateTime>
        {
            new DateTime(2026, 9, 10),
            new DateTime(2026, 9, 15),
            new DateTime(2026, 9, 20)
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                // Jeśli data jest na liście, zwracamy czerwony kolor (lub inny)
                if (HighlightedDates.Contains(date.Date))
                {
                    return Brushes.LightCoral;
                }
            }
            return Brushes.Transparent; // Domyślne tło
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
