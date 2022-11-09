using System.Collections.Generic;

namespace Pedometer.Entities
{
    public class User
    {
        public string Name { get; }
        public List<DaySteps> Steps { get; }

        public double AverageSteps { get; set; }
        public double BestStepsResult { get; set; }
        public double WorstStepsResult { get; set; }
        public User(string name)
        {
            Name = name;
            Steps = new List<DaySteps>();
            AverageSteps = 0;
            BestStepsResult = 0;
            WorstStepsResult = 0;
        }
    }
}
