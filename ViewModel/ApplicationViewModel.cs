using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Pedometer.Model;
using Pedometer.Entities;

namespace Pedometer.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Command openFileCommand;
        private Command printDataCommand;

        private Person selectedPerson;

        private IFileService file;
        private IDialogService dialog;
        
        private ObservableCollection<User> Users { get; set; }

        public ObservableCollection<Person> People { get; set; }

        public ApplicationViewModel(IFileService fileService, IDialogService dialogService)
        {
            file = fileService;
            dialog = dialogService;

            People = new ObservableCollection<Person>();

            Users = new ObservableCollection<User>();
        }

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

        public Command PrintDataCommand
        {
            get
            {
                if(printDataCommand == null)
                {
                    return printDataCommand = new Command(PrintData);
                }
                else
                {
                    return printDataCommand;
                }
            }
        }

        private void OpenFile()
        {

            try
            {
                if (dialog.OpenFile() == true)
                {
                    var people = file.Open(dialog.FilePath);

                    //People.Clear();
                    foreach(Person person in people)
                    {
                        People.Add(person);
                    }
                   
                    dialog.ShowMessage("Файл открыт успешно");
                }
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message);
            }

        }

        private void PrintData()
        {
            foreach (Person p in People)
            {
                if (!Users.Any(x => x.Name == p.User))
                {
                    var user = new User(p.User);
                    user.Steps.Add(new DaySteps(p.Steps));
                    Users.Add(user);
                }
                else
                {
                    var user = Users.FirstOrDefault(x => x.Name == p.User);
                    user.Steps.Add(new DaySteps(p.Steps));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
