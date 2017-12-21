////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Helpers;
using CI.QuickSave.Core.Security;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Storage;

namespace CI.QuickSave
{
    public class QuickSaveWriter
    {
        private readonly string _root;
        private readonly QuickSaveSettings _settings;

        private Dictionary<string, object> _items;

        private QuickSaveWriter(string root, QuickSaveSettings settings)
        {
            _root = root;
            _settings = settings;
        }

        /// <summary>
        /// Creates a QuickSaveWriter on the specified root
        /// </summary>
        /// <param name="root">The root to write to</param>
        /// <returns>A QuickSaveWriter instance</returns>
        public static QuickSaveWriter Create(string root)
        {
            return Create(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Attempts to save an object to a root under the specified key
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
        /// Attempts to save an object to a root under the specified key using the specified settings
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
        /// Creates a QuickSaveWriter on the specified root using the specified settings
        /// </summary>
        /// <param name="root">The root to write to</param>
        /// <param name="settings">Settings</param>
        /// <returns>A QuickSaveWriter instance</returns>
        public static QuickSaveWriter Create(string root, QuickSaveSettings settings)
        {
            QuickSaveWriter quickSaveWriter = new QuickSaveWriter(root, settings);
            quickSaveWriter.Open();
            return quickSaveWriter;
        }

        /// <summary>
        /// Writes an object to the specified key - you must called commit to write the data to file
        /// </summary>
        /// <typeparam name="T">The type of object to write</typeparam>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <returns>The QuickSaveWriter</returns>
        public QuickSaveWriter Write<T>(string key, T value)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
            }

            _items.Add(key, TypeHelper.ReplaceIfUnityType(value));

            return this;
        }

        /// <summary>
        /// Deletes the specified key if it exists
        /// </summary>
        /// <param name="key">The key to delete</param>
        public void Delete(string key)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
            }
        }

        /// <summary>
        /// Determines if the specified key exists
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>Does the key exist</returns>
        public bool Exists(string key)
        {
            return _items.ContainsKey(key);
        }

        /// <summary>
        /// Gets the names of all the keys
        /// </summary>
        /// <returns>A collection of key names</returns>
        public IEnumerable<string> GetAllKeys()
        {
            return _items.Keys.ToList();
        }

        /// <summary>
        /// Commits the changes to file
        /// </summary>
        public void Commit()
        {
            string jsonToSave;

            try
            {
                jsonToSave = JsonSerialiser.Serialise(_items);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Json serialisation failed", e);
            }

            string encryptedJson;

            try
            {
                encryptedJson = Cryptography.Encrypt(jsonToSave, _settings.SecurityMode, _settings.Password);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Encryption failed", e);
            }

            if(!FileAccess.SaveString(_root, false, encryptedJson))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }

        /// <summary>
        /// Attempts to commit the changes to file
        /// </summary>
        /// <returns>Was the commit successful</returns>
        public bool TryCommit()
        {
            try
            {
                string jsonToSave = JsonSerialiser.Serialise(_items);

                string encryptedJson = Cryptography.Encrypt(jsonToSave, _settings.SecurityMode, _settings.Password);

                FileAccess.SaveString(_root, false, encryptedJson);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Open()
        {
            string fileJson = FileAccess.LoadString(_root, false);

            if(string.IsNullOrEmpty(fileJson))
            {
                _items = new Dictionary<string, object>();

                return;
            }

            string decryptedJson;

            try
            {
                decryptedJson = Cryptography.Decrypt(fileJson, _settings.SecurityMode, _settings.Password);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Decryption failed", e);
            }

            try
            {
                _items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson) ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Failed to deserialise json", e);
            }
        }
    }
}