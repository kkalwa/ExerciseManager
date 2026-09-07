using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ExerciseManager.Commands
{
    public class CalendarHighlighter
    {
        public static DependencyProperty HighlightedDatesProperty =
            DependencyProperty.RegisterAttached("HighlightedDates",
                typeof(Collection<DateTime>),
                typeof(CalendarHighlighter),
                new UIPropertyMetadata(HighlightedDatesChanged));

        private static void HighlightedDatesChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            /* if (target == null)
                 return;

             Calendar calendar = target as Calendar;
             Collection<DateTime> dateSet = GetHighlightedDates(calendar);
             dateSet = new Collection<DateTime>(dateSet.Select(date => date.Date).ToList());

             var buttons = FindVisualChildren<CalendarDayButton>(calendar);
             foreach (var button in buttons)
             {
                 if (button.DataContext is DateTime buttonDate)
                 {
                     if (dateSet.Contains(buttonDate.Date))
                     {
                         // Apply your highlighted style directly
                         button.Background = Brushes.LightGreen;
                         button.FontWeight = FontWeights.Bold;
                     }
                     else
                     {
                         // Reset to default style if it doesn't match
                         button.ClearValue(Control.BackgroundProperty);
                         button.ClearValue(Control.FontWeightProperty);
                     }
                 }
             }*/
            Calendar calendar = target as Calendar;
            // Odepnij stare zdarzenie, aby uniknąć wycieków pamięci
            calendar.DisplayDateChanged -= Calendar_DisplayDateChanged;
            calendar.DisplayModeChanged -= Calendar_DisplayModeChanged;

            // Przypisz zdarzenie, które wychwyci zmianę miesiąca/roku
            calendar.DisplayDateChanged += Calendar_DisplayDateChanged;
            calendar.DisplayModeChanged -= Calendar_DisplayModeChanged;

            // Odśwież daty dla aktualnego widoku
            RefreshHighlights(calendar, e.NewValue as IEnumerable<DateTime>);

        }

        private static void Calendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if (sender is Calendar calendar && calendar.DisplayMode == CalendarMode.Month)
            {
                // Kiedy użytkownik klikał w nagłówek, a teraz wybrał miesiąc z widoku rocznego,
                // musimy wymusić przerysowanie wyróżnień na nowej siatce dni.
                // Czasami UI kalendarza potrzebuje ułamka sekundy na wygenerowanie przycisków dni,
                // dlatego bezpiecznie jest wywołać odświeżenie przez Dispatcher.
                calendar.Dispatcher.BeginInvoke(new Action(() => RefreshHighlights(calendar)),
                                                System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }

        private static void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                var dates = GetHighlightedDates(calendar);
                RefreshHighlights(calendar, dates);
            }
        }
        

        private static void RefreshHighlights(Calendar calendar, IEnumerable<DateTime> dates)
        {
            if (dates == null) return;

            Collection<DateTime> dateSet = GetHighlightedDates(calendar);
            dateSet = new Collection<DateTime>(dateSet.Select(date => date.Date).ToList());

            var buttons = FindVisualChildren<CalendarDayButton>(calendar);
            foreach (var button in buttons)
            {
                if (button.DataContext is DateTime buttonDate)
                {
                    if (dateSet.Contains(buttonDate.Date))
                    {
                        // Apply your highlighted style directly
                        button.Background = Brushes.LightGreen;
                        button.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        // Reset to default style if it doesn't match
                        button.ClearValue(Control.BackgroundProperty);
                        button.ClearValue(Control.FontWeightProperty);
                    }
                }
            }
        }

        public static void SetHighlightedDates(DependencyObject target, Collection<DateTime> value)
        {
            target.SetValue(HighlightedDatesProperty, value);
        }

        public static Collection<DateTime> GetHighlightedDates(DependencyObject target)
        {
            return (Collection<DateTime>)target.GetValue(HighlightedDatesProperty);
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;

                foreach (T childOfChild in FindVisualChildren<T>(child)) yield return childOfChild;
            }
        }
    }
}
