using Pedometer.Model;
using Pedometer.Services.Files.Export;
using Pedometer.ViewModel;
using System.Windows;

namespace Pedometer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel(new JsonFileParsing(), new DefaultDialogService(), new FileExport());
        }
    }
}
