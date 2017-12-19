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
using Windows.Storage;

namespace CI.QuickSave.Core
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

        public bool SaveString(string filename, string value)
        {
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

        public string LoadBytes(string filename)
        {
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