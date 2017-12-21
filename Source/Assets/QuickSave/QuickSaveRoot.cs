////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using CI.QuickSave.Core.Helpers;
using CI.QuickSave.Core.Security;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Storage;

namespace CI.QuickSave
{
    public static class QuickSaveRoot
    {
        /// <summary>
        /// Saves an object to a root, overwriting if it exists
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        public static void Save<T>(string root, T value)
        {
            Save(root, value, new QuickSaveSettings());
        }

        /// <summary>
        /// Saves an object to a root using the specified settings, overwriting if it exists
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <param name="settings">Settings</param>
        public static void Save<T>(string root, T value, QuickSaveSettings settings)
        {
            string jsonToSave;

            try
            {
                jsonToSave = JsonSerialiser.Serialise(TypeHelper.ReplaceIfUnityType(value));
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Json serialisation failed", e);
            }

            string encryptedJson;

            try
            {
                encryptedJson = Cryptography.Encrypt(jsonToSave, settings.SecurityMode, settings.Password);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Encryption failed", e);
            }

            if (!FileAccess.SaveString(root, false, encryptedJson))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }

        /// <summary>
        /// Loads the contents of the root into the specified object
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <returns>The object that was loaded</returns>
        public static T Load<T>(string root)
        {
            return Load<T>(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Loads the contents of the root into the specified object using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <param name="settings">Settings</param>
        /// <returns>The object that was loaded</returns>
        public static T Load<T>(string root, QuickSaveSettings settings)
        {
            string fileJson = FileAccess.LoadString(root, false);

            if (string.IsNullOrEmpty(fileJson))
            {
                throw new QuickSaveException("File either does not exist or is empty");
            }

            string decryptedJson;

            try
            {
                decryptedJson = Cryptography.Decrypt(fileJson, settings.SecurityMode, settings.Password);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Decryption failed", e);
            }

            try
            {
                return JsonSerialiser.Deserialise<T>(decryptedJson);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Failed to deserialise json", e);
            }
        }

        /// <summary>
        /// Deletes the specified root if it exists
        /// </summary>
        /// <param name="root">The root to delete</param>
        public static void Delete(string root)
        {
            FileAccess.Delete(root, false);
        }

        /// <summary>
        /// Determines if the specified root exist
        /// </summary>
        /// <param name="root">The root to check</param>
        /// <returns>Does the root exist</returns>
        public static bool Exists(string root)
        {
            return FileAccess.Exists(root, false);
        }

        /// <summary>
        /// Gets the names of all roots that have been saved
        /// </summary>
        /// <returns>A collection of root names</returns>
        public static IEnumerable<string> GetAllRoots()
        {
            return FileAccess.Files(false);
        }
    }
}