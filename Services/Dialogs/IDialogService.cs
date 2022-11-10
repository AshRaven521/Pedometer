namespace Pedometer.Model
{
    public interface IDialogService
    {
        void ShowMessage(string message);

        void ShowErrorMessage(string errMessage);
        bool OpenFile();
        bool SaveFile();
        string[] FilePaths { get; set; }
    }
}
