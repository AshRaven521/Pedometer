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
        //private Command showDataCommand;

        private Person selectedPerson;

        private IFileService file;
        private IDialogService dialog;

        private uint daysForAnalyzing;

        private ObservableCollection<User> users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                if (users == value)
                {
                    return;
                }
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private ObservableCollection<Person> People { get; set; }

        public ApplicationViewModel(IFileService fileService, IDialogService dialogService)
        {
            file = fileService;
            dialog = dialogService;

            daysForAnalyzing = 5;

            People = new ObservableCollection<Person>();

            //Users = new ObservableCollection<User>();
        }

        public Command CollectDataCommand
        {
            get
            {
                if (collectDataCommand is null)
                {
                    return collectDataCommand = new Command(PrintData);
                }
                else
                {
                    return collectDataCommand;
                }
            }
        }

        //public Command ShowDataCommand
        //{
        //    get
        //    {
        //        if (showDataCommand is null)
        //        {
        //            return showDataCommand = new Command(CollectData); 
        //        }
        //        else
        //        {
        //            return showDataCommand;
        //        }
        //    }
        //}


        private void OpenFile()
        {

            try
            {
                //Задаем количество дней, по которым будем проводить анализ количества шагов(кол-во считываемых json файлов)
                var people = file.Open(daysForAnalyzing);

                    
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
                dialog.ShowErrorMessage(ex.Message);
            }

        }

        public void CollectData()
        {
            //List<User> users = new List<User>();
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

            foreach (var user in users)
            {
                CalculateSteps(user);
            }

            //return users;
        }

        private void CalculateSteps(User user)
        {
            //double averageSteps = 0.0;
            var allUserDays = user.Steps.Select(x => x.Steps);
            user.AverageSteps = allUserDays.Average();
            user.BestStepsResult = allUserDays.Max();
            user.WorstStepsResult = allUserDays.Min();

            //foreach (var step in user.Steps)
            //{
            //    allUserSteps += step.Steps;
            //}
            //double averageSteps = allUserSteps / daysForAnalyzing;
        }

        private void FillDataTable()
        {

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
