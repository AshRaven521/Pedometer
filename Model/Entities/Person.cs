﻿using Pedometer.Entities;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Pedometer
{
    public class Person
    {
        public int Rank { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        [XmlArray("DaysSteps"), XmlArrayItem(Type = typeof(DaySteps))]
        public List<DaySteps> DaysSteps { get; }
        public int Steps { get; set; }

        public int AverageSteps { get; set; }
        public int BestStepsResult { get; set; }
        public int WorstStepsResult { get; set; }
        public bool IsLight { get; set; }
        public Person(string user)
        {
            User = user;
            DaysSteps = new List<DaySteps>();
            AverageSteps = 0;
            BestStepsResult = 0;
            WorstStepsResult = 0;
            IsLight = false;
        }

        // Конструктор без параметров для XML сериализации
        public Person()
        {

        }
    }
}
