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
            if (target == null)
                return;
            
            Calendar calendar = target as Calendar;
            Collection<DateTime> dateSet = GetHighlightedDates(calendar);
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
