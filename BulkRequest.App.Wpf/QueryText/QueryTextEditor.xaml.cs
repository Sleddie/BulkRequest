using System.Windows.Controls;

namespace BulkRequest.App.Wpf
{
    /// <summary>
    /// Interaction logic for QueryTextEditor.xaml
    /// </summary>
    public partial class QueryTextEditor : UserControl
    {
        public QueryTextEditor()
        {
            InitializeComponent();
        }

        internal RichTextBox QueryTextRichTextBox => _queryTextRichTextBox;
        internal CheckBox DefaultSettingsCheckBox => _defaultSettingsCheckBox;
        internal TextBox LocaleMarkTextBox => _localeMarkTextBox;
        internal TextBox KernelMarkTextBox => _kernelMarkTextBox;
    }
}