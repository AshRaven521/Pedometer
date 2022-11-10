using Pedometer.Entities;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Pedometer.Model
{
    public class JsonFileParsing : IFileService
    {
        public List<List<Person>> Open(uint days, string[] jsonFilePaths)
        {
            List<List<Person>> users = new List<List<Person>>();
            string json = string.Empty;

            for (int i = 0; i < days; i++)
            {
                using (StreamReader files = new StreamReader(jsonFilePaths[i]))
                {
                    json = files.ReadToEnd();
                    users.Add(JsonSerializer.Deserialize<List<Person>>(json));
                }
            }

            return users;
        }
    }
}
