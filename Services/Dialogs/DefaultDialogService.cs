using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pedometer.Model
{
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация",  MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowErrorMessage(string errMessage)
        {
            MessageBox.Show(errMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
