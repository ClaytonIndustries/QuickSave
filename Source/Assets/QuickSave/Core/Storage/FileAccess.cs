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
    public static class FileAccess
    {
        private static readonly string _defaultExtension = ".json";

#if !NETFX_CORE
        private static IFileAccess _storage = new FileAccessMono();
#else
        private static IFileAccess _storage = new FileAccessUWP();
#endif

        public static bool SaveString(string filename, bool includesExtension, string value)
        {
            return _storage.SaveString(GetFilenameWithExtension(filename, includesExtension), value);
        }

        public static bool SaveBytes(string filename, bool includesExtension, byte[] value)
        {
            return _storage.SaveBytes(GetFilenameWithExtension(filename, includesExtension), value);
        }

        public static string LoadString(string filename, bool includesExtension)
        {
            return _storage.LoadString(GetFilenameWithExtension(filename, includesExtension));
        }

        public static byte[] LoadBytes(string filename, bool includesExtension)
        {
            return _storage.LoadBytes(GetFilenameWithExtension(filename, includesExtension));
        }

        public static void Delete(string filename, bool includesExtension)
        {
            _storage.Delete(GetFilenameWithExtension(filename, includesExtension));
        }

        public static bool Exists(string filename, bool includesExtension)
        {
            return _storage.Exists(GetFilenameWithExtension(filename, includesExtension));
        }

        private static string GetFilenameWithExtension(string filename, bool includesExtension)
        {
            return includesExtension ? filename : filename + _defaultExtension;
        }

        public static IEnumerable<string> Files(bool includeExtensions)
        {
            return _storage.Files(includeExtensions);
        }
    }
}