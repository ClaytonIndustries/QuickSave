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
using CI.QuickSave.Core.Security;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Storage;

namespace CI.QuickSave
{
    public class QuickSaveReader
    {
        private readonly string _root;
        private readonly QuickSaveSettings _settings;

        private Dictionary<string, object> _items;

        private QuickSaveReader(string root, QuickSaveSettings settings)
        {
            _root = root;
            _settings = settings;
        }

        /// <summary>
        /// Creates a QuickSaveReader on the specified root
        /// </summary>
        /// <param name="root">The root to read from</param>
        /// <returns>A QuickSaveReader instance</returns>
        public static QuickSaveReader Create(string root)
        {
            return Create(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Creates a QuickSaveReader on the specified root using the specified settings
        /// </summary>
        /// <param name="root">The root to read from</param>
        /// <param name="settings">Settings</param>
        /// <returns>A QuickSaveReader instance</returns>
        public static QuickSaveReader Create(string root, QuickSaveSettings settings)
        {
            QuickSaveReader quickSaveReader = new QuickSaveReader(root, settings);
            quickSaveReader.Open();
            return quickSaveReader;
        }

        /// <summary>
        /// Attempts to load an object from a root under the specified key
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
        /// Attempts to load an object from a root under the specified key using the specified settings
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

                Dictionary<string, object> items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson) ?? new Dictionary<string, object>();

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
        /// Reads an object under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <returns>The object that was loaded</returns>
        public T Read<T>(string key)
        {
            if (!_items.ContainsKey(key))
            {
                throw new QuickSaveException("Key does not exists");
            }

            try
            {
                string propertyJson = JsonSerialiser.Serialise(_items[key]);

                return JsonSerialiser.Deserialise<T>(propertyJson);
            }
            catch
            {
                throw new QuickSaveException("Unable to deserialise json");
            }
        }

        /// <summary>
        /// Reads an object under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <param name="result">An action to be called when the read completes</param>
        /// <returns>The QuickSaveReader</returns>
        public QuickSaveReader Read<T>(string key, Action<T> result)
        {
            if (!_items.ContainsKey(key))
            {
                throw new QuickSaveException("Key does not exists");
            }

            try
            {
                string propertyJson = JsonSerialiser.Serialise(_items[key]);

                result(JsonSerialiser.Deserialise<T>(propertyJson));
            }
            catch
            {
                throw new QuickSaveException("Unable to deserialise json");
            }

            return this;
        }

        /// <summary>
        /// Attempts to read an object under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <param name="result">The object that was loaded</param>
        /// <returns>Was the read successful</returns>
        public bool TryRead<T>(string key, out T result)
        {
            result = default(T);

            if (!_items.ContainsKey(key))
            {
                return false;
            }

            try
            {
                string propertyJson = JsonSerialiser.Serialise(_items[key]);

                result = JsonSerialiser.Deserialise<T>(propertyJson);

                return true;
            }
            catch
            {
                return false;
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

        private void Open()
        {
            string fileJson = FileAccess.LoadString(_root, false);

            if (string.IsNullOrEmpty(fileJson))
            {
                throw new QuickSaveException("Root does not exist");
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
                _items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Failed to deserialise json", e);
            }
        }
    }
}