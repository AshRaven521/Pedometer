using System.Xml.Serialization;

namespace Pedometer.Entities
{
    [XmlRoot("Person")]
    public class DaySteps
    {
        //[XmlAttribute(AttributeName="Steps")]
        [XmlText]
        public int Steps { get; }
        public DaySteps(int steps)
        {
            Steps = steps;
        }

        // Конструктор без параметров для XML сериализации
        public DaySteps()
        {

        }
    }
}
