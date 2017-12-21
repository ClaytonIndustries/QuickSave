////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace CI.QuickSave.Core.Storage
{
    public interface IFileAccess
    {
        bool SaveString(string filename, string value);
        bool SaveBytes(string filename, byte[] value);
        string LoadString(string filename);
        byte[] LoadBytes(string filename);
        void Delete(string filename);
        bool Exists(string filename);
        IEnumerable<string> Files(bool includeExtensions);
    }
}