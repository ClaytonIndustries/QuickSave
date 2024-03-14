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
        public static string BasePath => Path.Combine(QuickSaveGlobalSettings.StorageLocation, "QuickSave");

        public static bool SaveString(string filename, string value)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
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

        public static bool SaveBytes(string filename, byte[] value)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
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

        public static bool SaveLines(string filename, List<string> lines)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static string LoadString(string filename)
        {
            try
            {
                if (Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
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

        public static byte[] LoadBytes(string filename)
        {
            try
            {
                if (Exists(filename))
                {
                    using (FileStream fileStream = new FileStream(filename, FileMode.Open))
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

        public static List<string> LoadLines(string filename)
        {
            try
            {
                if (Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        List<string> lines = new List<string>();
                        string line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                        return lines;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public static void Delete(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
            catch
            {
            }
        }

        public static bool Exists(string filename) => File.Exists(filename);

        public static IEnumerable<string> GetFiles(string directory)
        {
            try
            {
                return Directory.GetFiles(directory).Select(x => Path.GetFileName(x));
            }
            catch
            {
            }

            return new List<string>();
        }

        public static string GetPathFromBase(string filename) => Path.Combine(BasePath, $"{filename}.json");

        public static void CreateRootFolder()
        {
            try
            {
                if (!Directory.Exists(BasePath))
                {
                    Directory.CreateDirectory(BasePath);
                }
            }
            catch
            {
            }
        }
    }
}