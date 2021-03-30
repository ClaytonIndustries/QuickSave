////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CI.QuickSave.Core.Storage
{
    public static class FileAccess
    {
        private const string _defaultExtension = ".json";

        private static string BasePath => Path.Combine(QuickSaveGlobalSettings.StorageLocation, "QuickSave");

        public static bool SaveString(string filename, bool includesExtension, string value)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                using (StreamWriter writer = new StreamWriter(Path.Combine(BasePath, filename)))
                {
                    writer.Write(value);
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static bool SaveBytes(string filename, bool includesExtension, byte[] value)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                using (FileStream fileStream = new FileStream(Path.Combine(BasePath, filename), FileMode.Create))
                {
                    fileStream.Write(value, 0, value.Length);
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static string LoadString(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                if (Exists(filename, true))
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(BasePath, filename)))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public static byte[] LoadBytes(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                if (Exists(filename, true))
                {
                    using (FileStream fileStream = new FileStream(Path.Combine(BasePath, filename), FileMode.Open))
                    {
                        byte[] buffer = new byte[fileStream.Length];

                        fileStream.Read(buffer, 0, buffer.Length);

                        return buffer;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public static void Delete(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                string fileLocation = Path.Combine(BasePath, filename);

                File.Delete(fileLocation);
            }
            catch
            {
            }
        }

        public static bool Exists(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            string fileLocation = Path.Combine(BasePath, filename);

            return File.Exists(fileLocation);
        }

        public static IEnumerable<string> Files(bool includeExtensions)
        {
            try
            {
                CreateRootFolder();

                if (includeExtensions)
                {
                    return Directory.GetFiles(BasePath, "*.json").Select(x => Path.GetFileName(x));
                }
                else
                {
                    return Directory.GetFiles(BasePath, "*.json").Select(x => Path.GetFileNameWithoutExtension(x));
                }
            }
            catch
            {
            }

            return new List<string>();
        }

        private static string GetFilenameWithExtension(string filename, bool includesExtension)
        {
            return includesExtension ? filename : filename + _defaultExtension;
        }

        private static void CreateRootFolder()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
        }
    }
}