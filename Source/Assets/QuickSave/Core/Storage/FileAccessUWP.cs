////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;

namespace CI.QuickSave.Core.Storage
{
    public class FileAccessUWP : IFileAccess
    {
        private static StorageFolder _baseFolder;

        public bool SaveString(string filename, string value)
        {
            try
            {
                CreateRootFolder();

                StorageFile file = _baseFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting).AsTask().Result;

                FileIO.WriteTextAsync(file, value).AsTask().Wait();

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

                StorageFile file = _baseFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting).AsTask().Result;

                FileIO.WriteBytesAsync(file, value).AsTask().Wait();

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

                StorageFile file = _baseFolder.GetFileAsync(filename).AsTask().Result;

                return FileIO.ReadTextAsync(file).AsTask().Result;
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

                StorageFile file = _baseFolder.GetFileAsync(filename).AsTask().Result;

                return FileIO.ReadBufferAsync(file).AsTask().Result.ToArray();
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

                StorageFile file = _baseFolder.GetFileAsync(filename).AsTask().Result;

                file.DeleteAsync().AsTask().Wait();
            }
            catch
            {
            }
        }

        public bool Exists(string filename)
        {
            try
            {
                CreateRootFolder();

                _baseFolder.GetFileAsync(filename).AsTask().Wait();

                return true;
            }
            catch
            {
            }

            return false;
        }

        public IEnumerable<string> Files(bool includeExtensions)
        {
            try
            {
                CreateRootFolder();

                if (includeExtensions)
                {
                    return _baseFolder.GetFilesAsync().AsTask().Result.Where(x => x.FileType == ".json").Select(x => x.Name).ToList();
                }
                else
                {
                    return _baseFolder.GetFilesAsync().AsTask().Result.Select(x => x.DisplayName).ToList();
                }
            }
            catch
            {
            }

            return new List<string>();
        }

        private void CreateRootFolder()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            try
            {
                _baseFolder = folder.CreateFolderAsync("QuickSave", CreationCollisionOption.OpenIfExists).AsTask().Result;
            }
            catch
            {
            }        
        }
    }
}
#endif