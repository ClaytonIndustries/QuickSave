////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if !NETFX_CORE
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CI.QuickSave.Core
{
    public class FileAccessMono : IFileAccess
    {
        private static readonly string _basePath = Path.Combine(Application.persistentDataPath, "QuickSave");
        private static readonly string _extension = ".json";

        public bool Save(string filename, string value)
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

        public string Load(string filename)
        {
            try
            {
                CreateRootFolder();

                if (Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(_basePath, filename + _extension)))
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

        public void Delete(string filename)
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

        public bool Exists(string filename)
        {
            string fileLocation = Path.Combine(_basePath, filename + _extension);

            return File.Exists(fileLocation);
        }

        public IEnumerable<string> Files()
        {
            try
            {
                CreateRootFolder();

                return Directory.GetFiles(_basePath).Select(x => Path.GetFileNameWithoutExtension(x));

                //return new DirectoryInfo(_basePath).GetFiles().Select(x => Path.GetFileNameWithoutExtension(x.Name));
            }
            catch
            {
            }

            return new List<string>();
        }

        private void CreateRootFolder()
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }
    }
}
#endif