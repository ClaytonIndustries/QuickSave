using System.Collections.Generic;

namespace CI.QuickSave.Core
{
    public interface IFileAccess
    {
        void Save(string filename, string value);
        string Load(string filename);
        void Delete(string filename);
        bool Exists(string filename);
        IEnumerable<string> Files();
    }
}