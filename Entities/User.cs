using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedometer.Entities
{
    public class User
    {
        public string Name { get; }
        public List<DaySteps> Steps { get; }
        public User(string name)
        {
            Name = name;
            Steps = new List<DaySteps>();
        }
    }
}
