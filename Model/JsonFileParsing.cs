using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Pedometer.Model
{
    public class JsonFileParsing : IFileService
    {
        public List<List<Person>> Open(int daysCount)
        {
            List<List<Person>> people = new List<List<Person>>();
            string json = "";

            for(int i = 1; i <= daysCount; i++)
            {

                using(StreamReader files = new StreamReader($"day{i}.json"))
                {
                    json = files.ReadToEnd();
                    people.Add(JsonSerializer.Deserialize<List<Person>>(json));
                }
            }

            return people;
        }
    }
}
