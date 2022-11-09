using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedometer.Entities
{
    public class DaySteps
    {
        public int Steps { get; }
        public DaySteps(int steps)
        {
            Steps = steps;
        }
    }
}
