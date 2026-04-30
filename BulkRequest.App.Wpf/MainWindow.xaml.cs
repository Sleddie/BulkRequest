using System.Windows;

namespace BulkRequest.App.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            QueryTextEditing = new(_queryTextEditor);
        }

        private WpfQueryTextEditing QueryTextEditing { get; set; }
    }
}