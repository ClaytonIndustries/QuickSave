#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace CI.QuickSave.Core
{
    public class FileAccessUWP : IFileAccess
    {
        private static readonly string _extension = ".json";

        private static StorageFolder _baseFolder;

        public void Save(string filename, string value)
        {
            try
            {
                CreateRootFolder();

                StorageFile file = _baseFolder.CreateFileAsync(filename + _extension, CreationCollisionOption.ReplaceExisting).AsTask().Result;

                FileIO.WriteTextAsync(file, value).AsTask().Wait();
            }
            catch
            {
            }
        }

        public string Load(string filename)
        {
            try
            {
                CreateRootFolder();

                StorageFile file = _baseFolder.GetFileAsync(filename + _extension).AsTask().Result;

                return FileIO.ReadTextAsync(file).AsTask().Result;
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

                StorageFile file = _baseFolder.GetFileAsync(filename + _extension).AsTask().Result;

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

                _baseFolder.GetFileAsync(filename + _extension).AsTask().Wait();

                return true;
            }
            catch
            {
            }

            return false;
        }

        public IEnumerable<string> Files()
        {
            try
            {
                CreateRootFolder();

                return _baseFolder.GetFilesAsync().AsTask().Result.Select(x => x.DisplayName).ToList();
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