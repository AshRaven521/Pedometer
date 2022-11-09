using Pedometer.Entities;
using Pedometer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Pedometer.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private IFileService file;
        private IDialogService dialog;

        private uint daysForAnalyzing;

        private ChartValues<ObservablePoint> chartValues = new ChartValues<ObservablePoint>();

        private Command collectDataCommand;
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

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private User selectedUser;
        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                if (selectedUser == value)
                {
                    return;
                }
                SeriesCollection.Clear();
                chartValues.Clear();
                selectedUser = value;

                for (int i = 0; i < daysForAnalyzing; i++)
                {
                    // Добавляем новую точку, где X - день, Y - кол-во шагов пользователя за этот день
                    chartValues.Add(new ObservablePoint(i, selectedUser.Steps.ElementAt(i).Steps));

                    Labels.Add($"{i + 1}");
                }

                SeriesCollection.Add(new LineSeries
                {
                    Title = selectedUser.Name,
                    Values = chartValues
                });
                
                YFormatter = value => value.ToString("C");

                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ApplicationViewModel(IFileService fileService, IDialogService dialogService)
        {
            file = fileService;
            dialog = dialogService;

            daysForAnalyzing = 5;

            People = new ObservableCollection<Person>();

            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();    
        }


        private void OpenFile()
        {
            try
            {
                bool isOpen = dialog.OpenFile();
                daysForAnalyzing = (uint)dialog.FilePaths.Length;
                //Задаем количество дней, по которым будем проводить анализ количества шагов(кол-во считываемых json файлов)
                var people = file.Open(daysForAnalyzing, dialog.FilePaths);


                foreach (List<Person> listOfPerson in people)
                {
                    foreach (Person person in listOfPerson)
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
            var allUserDays = user.Steps.Select(x => x.Steps);
            user.AverageSteps = allUserDays.Average();
            user.BestStepsResult = allUserDays.Max();
            user.WorstStepsResult = allUserDays.Min();
        }

        private void PrintData()
        {
            CollectData();
            dialog.ShowMessage("Данные успешно собраны!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
