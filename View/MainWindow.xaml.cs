using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pedometer.ViewModel;
using Pedometer.Model;

namespace Pedometer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel(new JsonFileParsing(), new DefaultDialogService());
        }

        private void printDataButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel = new ApplicationViewModel(new JsonFileParsing(), new DefaultDialogService());
            var users = viewModel.CollectData();

            

            usersGrid.ItemsSource = viewModel.CollectData();
            usersGrid.Columns[0].Header = "Фамилия и Имя";
            usersGrid.Columns[1].Header = "Шаги";
        }
    }
}
