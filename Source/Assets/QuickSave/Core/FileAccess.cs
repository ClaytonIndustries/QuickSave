using System.IO;
using UnityEngine;

namespace CI.QuickSave.Core
{
    public static class FileAccess
    {
        private static readonly string _basePath = Path.Combine(Application.persistentDataPath, "SaveManager");
        private static readonly string _extension = ".json";

        public static bool Save(string filename, string value)
        {
            try
            {
                CreateRootFolder();

                using (StreamWriter writer = new StreamWriter(Path.Combine(_basePath, filename + _extension)))
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

        public static string Load(string filename)
        {
            try
            {
                CreateRootFolder();

                using (StreamReader reader = new StreamReader(Path.Combine(_basePath, filename + _extension)))
                {
                    return reader.ReadToEnd();
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
                CreateRootFolder();

                string fileLocation = Path.Combine(_basePath, filename + _extension);

                File.Delete(fileLocation);
            }
            catch
            {
            }
        }

        public static bool FileExists(string filename)
        {
            string fileLocation = Path.Combine(_basePath, filename + _extension);

            return File.Exists(fileLocation);
        }

        private static void CreateRootFolder()
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }
    }
}