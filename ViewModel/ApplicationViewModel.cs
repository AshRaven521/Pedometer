using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Pedometer.Entities;
using Pedometer.Model;
using Pedometer.Services.Files.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Pedometer.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private IFileService file;
        private IDialogService dialog;
        private IFileExportService fileExport;

        private uint daysForAnalyzing;

        private ChartValues<ObservablePoint> chartValues = new ChartValues<ObservablePoint>();

        private Command collectDataCommand;
        public Command CollectDataCommand
        {
            get
            {
                if (collectDataCommand is null)
                {
                    return collectDataCommand = new Command(ShowData);
                }
                else
                {
                    return collectDataCommand;
                }
            }
        }

        private Command exportDataCommand;
        public Command ExportDataCommand
        {
            get
            {
                if (exportDataCommand is null)
                {
                    return exportDataCommand = new Command(ExportData);
                }
                else
                {
                    return exportDataCommand;
                }
            }
        }
        private ObservableCollection<Person> people { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<Person> People
        {
            get
            {
                return people;
            }
            set
            {
                if (people == value)
                {
                    return;
                }
                people = value;
                OnPropertyChanged(nameof(People));
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private Person selectedPerson;
        public Person SelectedPerson
        {
            get
            {
                return selectedPerson;
            }
            set
            {
                if (selectedPerson == value)
                {
                    return;
                }
                selectedPerson = value;

                DrawChart();

                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public ApplicationViewModel(IFileService fileService, IDialogService dialogService, IFileExportService fileExportService)
        {
            file = fileService;
            dialog = dialogService;
            fileExport = fileExportService;

            daysForAnalyzing = 5;

            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();
        }


        private void FillPeopleCollection()
        {
            try
            {
                bool isOpen = dialog.OpenFile();
                daysForAnalyzing = (uint)dialog.FilePaths.Length;
                //Задаем количество дней, по которым будем проводить анализ количества шагов(кол-во считываемых json файлов)
                var peopleDays = file.Open(daysForAnalyzing, dialog.FilePaths);


                foreach (var day in peopleDays)
                {
                    foreach (var person in day)
                    {
                        // Обрабатываем добавление людей в ObservableCollection, чтобы они не повторялись
                        if (!people.Any(x => x.User == person.User))
                        {
                            var p = new Person(person.User);
                            p.DaysSteps.Add(new DaySteps(person.Steps));
                            p.Rank = person.Rank;
                            p.Status = person.Status;
                            p.IsLight = false;
                            people.Add(p);
                        }
                        else
                        {
                            var p = people.FirstOrDefault(x => x.User == person.User);
                            p.DaysSteps.Add(new DaySteps(person.Steps));
                        }
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
            FillPeopleCollection();

            foreach (var person in people)
            {
                CalculateProperties(person);
            }
        }

        private void CalculateProperties(Person person)
        {
            var allPersonDays = person.DaysSteps.Select(x => x.Steps);
            person.AverageSteps = Convert.ToInt32(allPersonDays.Average());
            person.BestStepsResult = allPersonDays.Max();
            person.WorstStepsResult = allPersonDays.Min();
            person.Steps = allPersonDays.Sum();

            if (person.BestStepsResult > person.AverageSteps * 1.2 || person.WorstStepsResult < person.AverageSteps * 0.8)
            {
                person.IsLight = true;
            }
        }

        private void ShowData()
        {
            CollectData();
            dialog.ShowMessage("Данные успешно собраны!");
        }
        private void ExportData()
        {
            bool isSave = dialog.SaveFile();

            // В массиве FilePaths будет только 1 элемент, т.к. при сохранении отключена возможность выбора нескольких файлов
            string filePath = dialog.FilePaths[0];

            fileExport.SaveToFile(filePath, selectedPerson);
        }

        private void DrawChart()
        {
            SeriesCollection.Clear();
            chartValues.Clear();
            Labels.Clear();

            for (int i = 0; i < daysForAnalyzing; i++)
            {
                if (i < selectedPerson.DaysSteps.Count)
                {
                    // Добавляем новую точку, где X - день, Y - кол-во шагов пользователя за этот день
                    chartValues.Add(new ObservablePoint(i, selectedPerson.DaysSteps.ElementAt(i).Steps));
                    Labels.Add($"{i + 1}");
                }
            }

            SeriesCollection.Add(new LineSeries
            {
                Title = selectedPerson.User,
                Values = chartValues,
                Foreground = Brushes.Black
            });

            YFormatter = value => value.ToString("C");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
