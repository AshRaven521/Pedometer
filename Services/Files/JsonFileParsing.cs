using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Pedometer.Model
{
    public class JsonFileParsing : IFileService
    {
        public List<List<Person>> Open(uint daysCount, string[] paths)
        {
            List<List<Person>> people = new List<List<Person>>();
            string json = "";

            for (int i = 0; i < daysCount; i++)
            {
                using (StreamReader files = new StreamReader(paths[i]))
                {
                    json = files.ReadToEnd();
                    people.Add(JsonSerializer.Deserialize<List<Person>>(json));
                }
            }

            return people;
        }
    }
}
