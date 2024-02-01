////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using CI.QuickSave.Core.Storage;
using CI.QuickSave.Core.Util;

namespace CI.QuickSave
{
    public class QuickSaveRaw
    {
        /// <summary>
        /// Saves a string directly to the specified file, overwriting if it already exists
        /// </summary>
        /// <param name="path">The file to save to</param>
        /// <param name="content">The string to save</param>
        public static void SaveString(string path, string content) => SaveString(path, content, new QuickSaveSettings());

        /// <summary>
        /// Saves a string directly to the specified file using the specified settings, overwriting if it already exists
        /// </summary>
        /// <param name="path">The file to save to</param>
        /// <param name="content">The string to save</param>
        /// <param name="settings">Settings</param>
        public static void SaveString(string path, string content, QuickSaveSettings settings)
        {
            var contentToWrite = DataService.PrepareSaveData(content, settings);

            if (!FileAccess.SaveString(path, contentToWrite))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }

        /// <summary>
        /// Saves a byte array directly to the specified file, overwriting if it already exists
        /// </summary>
        /// <param name="path">The file to save to</param>
        /// <param name="content">The byte array to save</param>
        public static void SaveBytes(string path, byte[] content)
        {
            if (!FileAccess.SaveBytes(path, content))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }

        /// <summary>
        /// Loads the contents of the specified file into a string
        /// </summary>
        /// <param name="path">The file to load from</param>
        /// <returns>The contents of the file as a string</returns>
        public static string LoadString(string path) => LoadString(path, new QuickSaveSettings());

        /// <summary>
        /// Loads the contents of the specified file into a string using the specified settings
        /// </summary>
        /// <param name="path">The file to load from</param>
        /// <param name="settings">Settings</param>
        /// <returns>The contents of the file as a string</returns>
        public static string LoadString(string path, QuickSaveSettings settings)
        {
            var content = FileAccess.LoadString(path);

            if (content == null)
            {
                throw new QuickSaveException("Failed to load file");
            }

            return DataService.PrepareLoadedData(content, settings);
        }

        /// <summary>
        /// Loads the contents of the specified file into a byte array
        /// </summary>
        /// <param name="path">The file to load from</param>
        /// <returns>The contents of the file as a byte array</returns>
        public static byte[] LoadBytes(string path)
        {
            byte[] content = FileAccess.LoadBytes(path);

            if (content == null)
            {
                throw new QuickSaveException("Failed to load file");
            }

            return content;
        }

        /// <summary>
        /// Loads an asset stored in a resources folder
        /// </summary>
        /// <typeparam name="T">The type of asset to load</typeparam>
        /// <param name="path">The path of the asset to load, relative to the Assets folder and without an extension</param>
        /// <returns>The specified asset</returns>
        public static T LoadResource<T>(string path) where T : UnityEngine.Object => UnityEngine.Resources.Load<T>(path);

        /// <summary>
        /// Deletes the specified file if it exists
        /// </summary>
        /// <param name="path">The file to delete</param>
        public static void Delete(string path) => FileAccess.Delete(path);

        /// <summary>
        /// Determines if the specified file exists
        /// </summary>
        /// <param name="path">The file to check</param>
        /// <returns>Does the file exist</returns>
        public static bool Exists(string path) => FileAccess.Exists(path);

        /// <summary>
        /// Gets the names of all files in the specified directory
        /// </summary>
        /// <param name="directory">The path of the directory</param>
        /// <returns>The filenames of all the files in the directory</returns>
        public static IEnumerable<string> GetAllFiles(string directory) => FileAccess.GetFiles(directory);
    }
}