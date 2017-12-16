﻿////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public static class QuickSaveManager
    {
        /// <summary>
        /// Attempt to save an object to a file under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <param name="replace">If the key already exists should it be overwitten</param>
        /// <returns>Was the save successful</returns>
        public static bool TrySave<T>(string root, string key, T value, bool replace)
        {
            return TrySave(root, key, value, replace, new QuickSaveSettings());
        }

        /// <summary>
        /// Attempt to save an object to a file under the specified key using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <param name="replace">If the key already exists should it be overwitten</param>
        /// <param name="settings">Settings</param>
        /// <returns>Was the save successful</returns>
        public static bool TrySave<T>(string root, string key, T value, bool replace, QuickSaveSettings settings)
        {
            try
            {
                string fileJson = FileAccess.Load(root);

                Dictionary<string, object> items = null;

                if (string.IsNullOrEmpty(fileJson))
                {
                    items = new Dictionary<string, object>();
                }
                else
                {
                    items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson) ?? new Dictionary<string, object>();

                    if (items.ContainsKey(key))
                    {
                        if (replace)
                        {
                            items.Remove(key);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                items.Add(key, value);

                string jsonToSave = JsonSerialiser.Serialise(items);

                string encryptedJson = Cryptography.Encrypt(jsonToSave, settings.SecurityMode, settings.Password);

                FileAccess.Save(root, encryptedJson);

                return true;
            }
            catch
            {
                return false;
            }  
        }

        /// <summary>
        /// Attempt to load an object from a file under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <param name="key">The key this object was saved under</param>
        /// <param name="result">The object that was loaded</param>
        /// <returns>Was the load successful</returns>
        public static bool TryLoad<T>(string root, string key, out T result)
        {
            return TryLoad(root, key, new QuickSaveSettings(), out result);
        }

        /// <summary>
        /// Attempt to load an object from a file under the specified key using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <param name="key">The key this object was saved under</param>
        /// <param name="settings">Settings</param>
        /// <param name="result">The object that was loaded</param>
        /// <returns>Was the load successful</returns>
        public static bool TryLoad<T>(string root, string key, QuickSaveSettings settings, out T result)
        {
            result = default(T);

            try
            {
                string fileJson = FileAccess.Load(root);

                if (string.IsNullOrEmpty(fileJson))
                {
                    return false;
                }

                string decryptedJson = Cryptography.Decrypt(fileJson, settings.SecurityMode, settings.Password);

                Dictionary<string, object>  items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson) ?? new Dictionary<string, object>();

                if (!items.ContainsKey(key))
                {
                    return false;
                }

                string propertyJson = JsonSerialiser.Serialise(items[key]);

                result = JsonSerialiser.Deserialise<T>(propertyJson);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete the specified root if it exists
        /// </summary>
        /// <param name="root">The root to delete</param>
        public static void Delete(string root)
        {
            FileAccess.Delete(root);
        }

        /// <summary>
        /// Does the specified root exist
        /// </summary>
        /// <param name="root">The root to check</param>
        /// <returns>Does the root exist</returns>
        public static bool Exists(string root)
        {
            return FileAccess.Exists(root);
        }

        /// <summary>
        /// Get the names of all roots
        /// </summary>
        /// <returns>A collection of root names</returns>
        public static IEnumerable<string> GetAllRoots()
        {
            return FileAccess.Files();
        }
    }
}