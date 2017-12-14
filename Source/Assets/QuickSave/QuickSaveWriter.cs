using System;
using System.Collections.Generic;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public class QuickSaveWriter
    {
        private readonly string _root;

        private Dictionary<string, object> _items;

        private QuickSaveWriter(string root)
        {
            _root = root;
        }

        public static QuickSaveWriter Create(string root)
        {
            QuickSaveWriter saveManagerWriter = new QuickSaveWriter(root);
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

                FileAccess.Save(_root, jsonToSave);
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
                _items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson) ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load data", e);
            }
        }
    }
}