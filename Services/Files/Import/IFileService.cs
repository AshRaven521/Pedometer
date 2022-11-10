using System.Collections.Generic;

namespace Pedometer.Model
{
    public interface IFileService
    {
        List<List<Person>> Open(uint days, string[] jsonFilePaths);

    }
}
