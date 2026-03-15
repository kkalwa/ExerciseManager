using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace ExerciseManager.Models
{
    public class ToTreeViewItemConverter
    {
        public static TreeViewItem ConvertToTreeViewItem(ExerciseSetModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            TreeViewItem output = new TreeViewItem()
            {
                Header = item.ExerciseSetTitle,
                IsExpanded = true
            };
            foreach (var e in item.Exercises)
            {
                TreeViewItem nestedItem = new TreeViewItem() { Header = e.ExerciseName };
                output.Items.Add(nestedItem);
            }
            return output;
        }

        public static ObservableCollection<TreeViewItem> ConvertToTreeViewItemList(ObservableCollection<ExerciseSetModel> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            ObservableCollection<TreeViewItem> output = new ObservableCollection<TreeViewItem>();
            foreach (var element in list)
            {
                output.Add(ConvertToTreeViewItem(element));
            }
            return output;
        }
    }
}
