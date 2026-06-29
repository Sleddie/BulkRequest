using System.Windows;
using System.Windows.Controls;

namespace BulkRequest.App.Wpf
{
    public partial class CustomCheckListBox
    {
        public static DependencyProperty OrientationProperty
        {
            get => DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(CustomCheckListBox));
        }
    }
}