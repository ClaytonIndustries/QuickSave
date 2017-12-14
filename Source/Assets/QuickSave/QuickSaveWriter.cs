using System;
using System.Collections.Generic;
using CI.QuickSave.Core;

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

        public static QuickSaveWriter Create(string root)
        {
            return Create(root, new QuickSaveSettings());
        }

        public static QuickSaveWriter Create(string root, QuickSaveSettings settings)
        {
            QuickSaveWriter saveManagerWriter = new QuickSaveWriter(root, settings);
            saveManagerWriter.Open();
            return saveManagerWriter;
        }

        public void Write<T>(string key, T value, bool replace)
        {
            if (_items.ContainsKey(key))
            {
                if(replace)
                {
                    _items.Remove(key);
                }
                else
                {
                    throw new ArgumentException("Key already exists");
                }
            }

            _items.Add(key, value);
        }

        public void Delete(string key)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
            }
        }

        public bool KeyExists(string key)
        {
            return _items.ContainsKey(key);
        }

        public void Commit()
        {
            try
            {
                string jsonToSave = JsonSerialiser.Serialise(_items);

                string encryptedJson = Cryptography.Encrypt(jsonToSave, _settings.SecurityMode, _settings.Password);

                FileAccess.Save(_root, encryptedJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to save data", e);
            }
        }

        private void Open()
        {
            string fileJson = FileAccess.Load(_root);

            if(string.IsNullOrEmpty(fileJson))
            {
                _items = new Dictionary<string, object>();

                return;
            }

            try
            {
                string decryptedJson = Cryptography.Decrypt(fileJson, _settings.SecurityMode, _settings.Password);

                _items = JsonSerialiser.Deserialise<Dictionary<string, object>>(decryptedJson) ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load data", e);
            }
        }
    }
}