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
    public static class FileAccess
    {
#if !NETFX_CORE
        private static IFileAccess _storage = new FileAccessMono();
#else
        private static IFileAccess _storage = new FileAccessUWP();
#endif

        public static void Save(string filename, string value)
        {
            _storage.Save(filename, value);
        }

        public static string Load(string filename)
        {
            return _storage.Load(filename);
        }

        public static void Delete(string filename)
        {
            _storage.Delete(filename);
        }

        public static bool Exists(string filename)
        {
            return _storage.Exists(filename);
        }

        public static IEnumerable<string> Files()
        {
            return _storage.Files();
        }
    }
}