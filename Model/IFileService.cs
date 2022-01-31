using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedometer.Model
{
    public interface IFileService
    {
        List<Person> Open(string fileName);

    }
}
