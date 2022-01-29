using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Pedometer.Model;

namespace Pedometer.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Command openFileCommand;

        public Command OpenFileCommand
        {
            get
            {
                if(openFileCommand == null)
                {
                    return openFileCommand = new Command(OpenFile);
                }
                else
                {
                    return openFileCommand;
                }
            }
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.ShowDialog();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
