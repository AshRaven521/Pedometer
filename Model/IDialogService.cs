using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedometer.Model
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        bool OpenFile();
        string FilePath { get; set; }
    }
}
