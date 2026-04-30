using BulkRequest.App.Base;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BulkRequest.App.Wpf
{
    public class WpfQueryTextEditing : QueryTextEditing
    {
        public WpfQueryTextEditing(QueryTextEditor control)
        {
            IsHandlersEnabled = false;
            Control = control;
            Control.DefaultSettingsCheckBox.Checked += IsCheckedChanged;
            Control.DefaultSettingsCheckBox.Unchecked += IsCheckedChanged;
            Control.QueryTextRichTextBox.TextChanged += TextChanged;
            Control.LocaleMarkTextBox.TextChanged += MarkChanged;
            Control.KernelMarkTextBox.TextChanged += MarkChanged;
            IsHandlersEnabled = true;
            CheckDefaultSettings();
            CheckAllMarks();
        }

        public override string Text
        {
            get
            {
                return QueryTextRange.Text;
            }
            set
            {
                QueryTextRange.Text = value ?? "";
            }
        }
        public override bool IsDefaultSettings
        {
            get => Control.DefaultSettingsCheckBox.IsChecked ??= true;
            set => Control.DefaultSettingsCheckBox.IsChecked = value;
        }
        public override string LocaleMark
        {
            get => Control.LocaleMarkTextBox.Text;
            set => Control.LocaleMarkTextBox.Text = value;
        }
        public override string KernelMark
        {
            get => Control.KernelMarkTextBox.Text;
            set => Control.KernelMarkTextBox.Text = value;
        }
        public override string MonthMark { get; set; } = "";
        public override string YearMark { get; set; } = "";
        private QueryTextEditor Control { get; set; }
        private FlowDocument TextDocument
        {
            get => Control.QueryTextRichTextBox.Document;
        }
        private TextRange QueryTextRange
        {
            get
            {
                return new(
                    TextDocument.ContentStart,
                    TextDocument.ContentEnd);
            }
        }

        protected override void HighlightMarksInTextControl()
        {
            Control.Dispatcher
                .InvokeAsync(QueryTextRange.ClearAllProperties);
            Control.Dispatcher
                .InvokeAsync(() =>
                TextDocument.HighlightWordsWithColour(Marks));
        }

        protected override void CheckDefaultSettings()
        {
            base.CheckDefaultSettings();
            bool isDefault = IsDefaultSettings;
            Control.LocaleMarkTextBox.IsEnabled = !isDefault;
            Control.KernelMarkTextBox.IsEnabled = !isDefault;
        }

        protected virtual void IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!IsHandlersEnabled)
                return;

            CheckDefaultSettings();
        }

        protected virtual void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsHandlersEnabled)
                return;

            CheckAllMarks();
        }

        protected virtual void MarkChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsHandlersEnabled)
                return;

            CheckAllMarks();
        }
    }
}