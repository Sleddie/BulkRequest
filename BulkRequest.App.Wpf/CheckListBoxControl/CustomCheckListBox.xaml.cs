using BulkRequest.App.Wpf.CheckListBoxControl;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BulkRequest.App.Wpf
{
    public partial class CustomCheckListBox : UserControl
    {
        public CustomCheckListBox()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        [Browsable(true)]
        [Category("Appearance")]
        public bool UseCheckBoxes
        {
            get;
            set
            {
                field = value;
                SetUseCheckBoxes(field);
            }
        }

        [Browsable(true)]
        [Category("Data")]
        public IEnumerable? ItemSource
        {
            get;
            set
            {
                field = value;
                SetItemSource(field);
            }
        }

        [Browsable(true)]
        [Category("Data")]
        public IEnumerable<CustomCheckListBoxItem> Items
        {
            get => _listBox.Items.OfType<CustomCheckListBoxItem>();
        }

        protected void SetUseCheckBoxes(bool useCheckBox)
        {
            _checkAllBox.Visibility = useCheckBox
                ? Visibility.Visible
                : Visibility.Collapsed;

            foreach (var listBoxItem in Items)
            {
                listBoxItem.UseCheckBox = useCheckBox;
            }
        }

        protected void SetItemSource(IEnumerable? source)
        {
            _listBox.Items.Clear();

            if (source is null)
            {
                return;
            }

            foreach (object sourceItem in source)
            {
                CustomCheckListBoxItem listBoxItem = new(sourceItem);
                _listBox.Items.Add(listBoxItem);
            }
        }

        private void CheckAllBox_CheckedChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}