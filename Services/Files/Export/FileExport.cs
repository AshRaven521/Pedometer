using CsvHelper;
using Pedometer.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Serialization;

namespace Pedometer.Services.Files.Export
{
    public class FileExport : IFileExportService
    {
        private IDialogService dialog = new DefaultDialogService();

        public void SaveToFile(string fileName, Person person)
        {
            string fileExtension = fileName.Split('.')[1];

            switch (fileExtension)
            {
                case "json":
                    SaveToJsonFile(fileName, person);
                    break;
                case "xml":
                    SaveToXmlFile(fileName, person);
                    break;
                case "csv":
                    SaveToCsvFile(fileName, person);
                    break;
                default:
                    dialog.ShowErrorMessage("Выбранный тип файла не поддерживается!");
                    break;
            }
        }

        private void SaveToJsonFile(string filePath, Person person)
        {
            if (person is null || person.User == string.Empty || string.IsNullOrWhiteSpace(person.User))
            {
                dialog.ShowErrorMessage("Не выбран пользователь!");
                return;
            }

            string json = string.Empty;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            try
            {
                using (var streamWriter = new StreamWriter(filePath))
                {
                    json = JsonSerializer.Serialize<Person>(person, options);
                    streamWriter.WriteLine(json);
                }
                dialog.ShowMessage("JSON файл был создан успешно!");
            }
            catch (Exception ex)
            {
                dialog.ShowErrorMessage($"Произошла ошибка при формировании JSON файла! {ex.Message}");
            }

        }

        private void SaveToXmlFile(string filePath, Person person)
        {
            if (person is null || person.User == string.Empty || string.IsNullOrWhiteSpace(person.User))
            {
                dialog.ShowErrorMessage("Не выбран пользователь!");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Person));
            try
            {
                using (var streamWriter = new StreamWriter(filePath))
                {
                    serializer.Serialize(streamWriter, person);
                }
                dialog.ShowMessage("XML файл был создан успешно!");
            }
            catch (Exception ex)
            {
                dialog.ShowErrorMessage($"Произошла ошибка при формировании XML файла! {ex.Message}");
            }

        }

        private void SaveToCsvFile(string filePath, Person person)
        {
            if (person is null || person.User == string.Empty || string.IsNullOrWhiteSpace(person.User))
            {
                dialog.ShowErrorMessage("Не выбран пользователь!");
                return;
            }

            var peopleList = new List<Person>();
            peopleList.Add(person);

            try
            {
                using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                    {
                        csvWriter.WriteRecords(peopleList);
                    }
                }

                dialog.ShowMessage("CSV файл был создан успешно!");
            }
            catch (Exception ex)
            {
                dialog.ShowErrorMessage($"Произошла ошибка при формировании CSV файла! {ex.Message}");
            }

        }
    }
}
