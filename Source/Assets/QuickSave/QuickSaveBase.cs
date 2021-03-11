using System;
using System.Collections.Generic;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Settings;
using CI.QuickSave.Core.Storage;

namespace CI.QuickSave
{
    public abstract class QuickSaveBase
    {
        protected Dictionary<string, object> _items;
        protected string _root;
        protected QuickSaveSettings _settings;

        protected void Open(bool rootMightNotExist)
        {
            var json = FileAccess.LoadString(_root, false);

            if (string.IsNullOrEmpty(json))
            {
                if (rootMightNotExist)
                {
                    _items = new Dictionary<string, object>();

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
                _items = JsonSerialiser.Deserialise<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Failed to deserialise json", e);
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
                throw new QuickSaveException("Json serialisation failed", e);
            }

            try
            {
                json = Compression.Compress(json, _settings.CompressionMode);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Encryption failed", e);
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