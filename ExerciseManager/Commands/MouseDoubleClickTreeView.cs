using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExerciseManager.Commands
{
    public class MouseDoubleClickTreeView
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseDoubleClickTreeView),
            new UIPropertyMetadata(CommandChanged));

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            TreeView control = target as TreeView;
            if (control != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    control.MouseDoubleClick += OnMouseDoubleClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    control.MouseDoubleClick -= OnMouseDoubleClick;
                }
            }
        }

        private static void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            TreeView control = sender as TreeView;
            ICommand command = (ICommand)control.GetValue(CommandProperty);
            command.Execute(control.SelectedItem);
        }
    }
}
