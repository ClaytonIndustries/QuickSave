using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Settings;
using CI.QuickSave.Core.Storage;
using Newtonsoft.Json.Linq;

namespace CI.QuickSave
{
    public abstract class QuickSaveBase
    {
        protected JObject _items;
        protected string _root;
        protected QuickSaveSettings _settings;

        /// <summary>
        /// Determines if the specified key exists
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>Does the key exist</returns>
        public bool Exists(string key)
        {
            return _items[key] != null;
        }

        /// <summary>
        /// Gets the names of all the keys under this root
        /// </summary>
        /// <returns>A collection of key names</returns>
        public IEnumerable<string> GetAllKeys()
        {
            return _items.Properties().Select(x => x.Name).ToList();
        }

        protected void Load(bool rootMightNotExist)
        {
            var json = FileAccess.LoadString(_root, false);

            if (string.IsNullOrEmpty(json))
            {
                if (rootMightNotExist)
                {
                    _items = new JObject();

                    return;
                }

                throw new QuickSaveException("Root does not exist");
            }

            // Gzip parses base64 anyway so no need to do it twice
            if (_settings.CompressionMode != CompressionMode.Gzip || _settings.SecurityMode != SecurityMode.Base64)
            {
                try
                {
                    json = Cryptography.Decrypt(json, _settings.SecurityMode, _settings.Password);
                }
                catch (Exception e)
                {
                    throw new QuickSaveException("Decryption failed", e);
                }
            }

            try
            {
                json = Compression.Decompress(json, _settings.CompressionMode);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Decompression failed", e);
            }

            try
            {
                _items = JObject.Parse(json);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Deserialisation failed", e);
            }
        }

        protected void Save()
        {
            string json;

            try
            {
                json = JsonSerialiser.Serialise(_items);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Serialisation failed", e);
            }

            try
            {
                json = Compression.Compress(json, _settings.CompressionMode);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Compression failed", e);
            }

            // Gzip outputs base64 anyway so no need to do it twice
            if (_settings.CompressionMode != CompressionMode.Gzip || _settings.SecurityMode != SecurityMode.Base64)
            {
                try
                {
                    json = Cryptography.Encrypt(json, _settings.SecurityMode, _settings.Password);
                }
                catch (Exception e)
                {
                    throw new QuickSaveException("Encryption failed", e);
                }
            }

            if (!FileAccess.SaveString(_root, false, json))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }
    }
}