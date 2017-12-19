////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public static class QuickSaveManager
    {
        /// <summary>
        /// Attempts to save an object to a file under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <returns>Was the save successful</returns>
        public static bool TrySave<T>(string root, string key, T value)
        {
            return TrySave(root, key, value, new QuickSaveSettings());
        }

        /// <summary>
        /// Attempts to save an object to a file under the specified key using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <param name="settings">Settings</param>
        /// <returns>Was the save successful</returns>
        public static bool TrySave<T>(string root, string key, T value, QuickSaveSettings settings)
        {
            try
            {
                string fileJson = FileAccess.LoadString(root, false);

                Dictionary<string, object> items = null;

                if (string.IsNullOrEmpty(fileJson))
                {
                    items = new Dictionary<string, object>();
                }
                else
                {
					string decryptedJson = Cryptography.Decrypt(fileJson, settings.SecurityMode, settings.Password);
					
                    items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson) ?? new Dictionary<string, object>();

                    if (items.ContainsKey(key))
                    {
                        items.Remove(key);
                    }
                }

                items.Add(key, TypeHelper.ReplaceIfUnityType(value));

                string jsonToSave = JsonSerialiser.Serialise(items);

                string encryptedJson = Cryptography.Encrypt(jsonToSave, settings.SecurityMode, settings.Password);

                FileAccess.SaveString(root, false, encryptedJson);

                return true;
            }
            catch
            {
                return false;
            }  
        }

        /// <summary>
        /// Attempts to load an object from a file under the specified key
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
        /// Attempts to load an object from a file under the specified key using the specified settings
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
                string fileJson = FileAccess.LoadString(root, false);

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
        /// Saves an object to a file, overwriting if it exists
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        public static void Save<T>(string root, T value)
        {
            Save(root, value, new QuickSaveSettings());
        }

        /// <summary>
        /// Saves an object to a file using the specified settings, overwriting if it exists
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
        /// Loads the contents of the file into the specified object
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <returns>The object that was loaded</returns>
        public static T Load<T>(string root)
        {
            return Load<T>(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Loads the contents of the file into the specified object using the specified settings
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
        /// Deletes the specified key from the specified root if it exists
        /// </summary>
        /// <param name="root">The root the key was saved under</param>
		/// <param name="key">The key to delete</param>
		public static void Delete(string root, string key)
		{
			Delete(root, key, new QuickSaveSettings());
		}
		
		/// <summary>
        /// Deletes the specified key from the specified root using the specified settings if it exists
        /// </summary>
        /// <param name="root">The root the key was saved under</param>
		/// <param name="key">The key to delete</param>
		/// <param name="settings">Settings</param>
		public static void Delete(string root, string key, QuickSaveSettings settings)
		{
			try
            {
                string fileJson = FileAccess.LoadString(root, false);
				
				if (string.IsNullOrEmpty(fileJson))
                {
                    return;
                }
				
				string decryptedJson = Cryptography.Decrypt(fileJson, settings.SecurityMode, settings.Password);
				
				Dictionary<string, object> items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson) ?? new Dictionary<string, object>();

				if (items.ContainsKey(key))
				{
					items.Remove(key);
				}

                string jsonToSave = JsonSerialiser.Serialise(items);

                string encryptedJson = Cryptography.Encrypt(jsonToSave, settings.SecurityMode, settings.Password);

                FileAccess.SaveString(root, false, encryptedJson);
            }
            catch
            {
            } 
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
        /// Gets the names of all roots
        /// </summary>
        /// <returns>A collection of root names</returns>
        public static IEnumerable<string> GetAllRoots()
        {
            return FileAccess.Files(true);
        }
    }
}