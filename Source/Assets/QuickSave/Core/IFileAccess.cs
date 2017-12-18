////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace CI.QuickSave.Core
{
    public interface IFileAccess
    {
        bool Save(string filename, string value);
        string Load(string filename);
        void Delete(string filename);
        bool Exists(string filename);
        IEnumerable<string> Files();
    }
}