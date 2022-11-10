namespace Pedometer.Services.Files.Export
{
    public interface IFileExportService
    {
        void SaveToFile(string fileName, Person person);

    }
}
