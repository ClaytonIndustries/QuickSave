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

namespace CI.QuickSave.Core.Storage
{
    public class FileAccessMono : IFileAccess
    {
        private static readonly string _basePath = Path.Combine(Application.persistentDataPath, "QuickSave");

        public bool SaveString(string filename, string value)
        {
            try
            {
                CreateRootFolder();

                using (StreamWriter writer = new StreamWriter(Path.Combine(_basePath, filename)))
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

        public bool SaveBytes(string filename, byte[] value)
        {
            try
            {
                CreateRootFolder();

                using (FileStream fileStream = new FileStream(Path.Combine(_basePath, filename), FileMode.Create))
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

        public string LoadString(string filename)
        {
            try
            {
                CreateRootFolder();

                if (Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(_basePath, filename)))
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

        public byte[] LoadBytes(string filename)
        {
            try
            {
                CreateRootFolder();

                if (Exists(filename))
                {
                    using (FileStream fileStream = new FileStream(Path.Combine(_basePath, filename), FileMode.Open))
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

        public void Delete(string filename)
        {
            try
            {
                CreateRootFolder();

                string fileLocation = Path.Combine(_basePath, filename);

                File.Delete(fileLocation);
            }
            catch
            {
            }
        }

        public bool Exists(string filename)
        {
            string fileLocation = Path.Combine(_basePath, filename);

            return File.Exists(fileLocation);
        }

        public IEnumerable<string> Files(bool includeExtensions)
        {
            try
            {
                CreateRootFolder();

                if(includeExtensions)
                {
                    return Directory.GetFiles(_basePath, "*.json").Select(x => Path.GetFileName(x));
                }
                else
                {
                    return Directory.GetFiles(_basePath, "*.json").Select(x => Path.GetFileNameWithoutExtension(x));
                }
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