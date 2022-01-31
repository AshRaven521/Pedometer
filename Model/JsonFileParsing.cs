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
        public List<Person> Open(string fileName)
        {
            List<Person> people = new List<Person>();
            string json = "";

            using(StreamReader files = new StreamReader(fileName))
            {
                json = files.ReadToEnd();
                people = JsonSerializer.Deserialize<List<Person>>(json);
            }

            return people;
        }
    }
}
