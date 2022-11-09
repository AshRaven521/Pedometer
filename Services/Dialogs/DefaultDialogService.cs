using Microsoft.Win32;
using System.Windows;

namespace Pedometer.Model
{
    public class DefaultDialogService : IDialogService
    {
        public string[] FilePaths { get; set; }

        public bool OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.Title = "Выберите файлы с информацией о пользователях и количестве их шагов";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                FilePaths = openFileDialog.FileNames;
                return true;
            }

            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowErrorMessage(string errMessage)
        {
            MessageBox.Show(errMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
