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
        private Command collectDataCommand;

        private Person selectedPerson;

        private IFileService file;
        private IDialogService dialog;
        
        public ObservableCollection<User> Users { get; set; }

        private ObservableCollection<Person> People { get; set; }

        public ApplicationViewModel(IFileService fileService, IDialogService dialogService)
        {
            file = fileService;
            dialog = dialogService;

            People = new ObservableCollection<Person>();

            Users = new ObservableCollection<User>();
        }

        public Command CollectDataCommand
        {
            get
            {
                if(collectDataCommand == null)
                {
                    return collectDataCommand = new Command(PrintData);
                }
                else
                {
                    return collectDataCommand;
                }
            }
        }


        private void OpenFile()
        {

            try
            {
                //Задаем количество дней, по которым будем проводить анализ количества шагов(кол-во считываемых json файлов)
                var people = file.Open(5);

                    
                foreach(List<Person> listOfPerson in people)
                {
                    foreach(Person person in listOfPerson)
                    {
                        People.Add(person);

                    }
                }
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message);
            }

        }

        public List<User> CollectData()
        {

            List<User> users = new List<User>();
            OpenFile();

            foreach (Person p in People)
            {
                if (!users.Any(x => x.Name == p.User))
                {
                    var user = new User(p.User);
                    user.Steps.Add(new DaySteps(p.Steps));
                    users.Add(user);
                }
                else
                {
                    var user = users.FirstOrDefault(x => x.Name == p.User);
                    user.Steps.Add(new DaySteps(p.Steps));
                }
            }

            return users;
        }

        private void PrintData()
        {
            CollectData();
            dialog.ShowMessage("Данные успешно собраны!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
