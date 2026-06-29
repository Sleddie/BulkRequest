using System.Windows;
using System.Windows.Controls;

namespace BulkRequest.App.Wpf.CheckListBoxControl
{
    public class CustomCheckListBoxItem : ContentControl
    {
        public CustomCheckListBoxItem(object sourceItem)
        {
            SourceItem = sourceItem;

            CheckBox = new()
            {
                VerticalContentAlignment = VerticalAlignment.Center
            };

            Label = new()
            {
                Padding = new Thickness(2, 0, 5, 0)
            };

            Panel = new()
            {
                Orientation = Orientation.Horizontal
            };
            Panel.Children.Add(CheckBox);
            Panel.Children.Add(Label);
        }

        public object SourceItem { get; private init; }
        public StackPanel Panel { get; private init; }
        public CheckBox CheckBox { get; private init; }
        public Label Label { get; private init; }

        public bool UseCheckBox
        {
            get;
            set
            {
                field = value;
                CheckBox.Visibility = field
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }
        public bool IsChecked
        {
            get => CheckBox.IsChecked ??= false;
            set => CheckBox.IsChecked = value;
        }
    }
}