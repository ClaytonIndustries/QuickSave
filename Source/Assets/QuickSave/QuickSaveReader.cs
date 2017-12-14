using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public class QuickSaveReader
    {
        private readonly string _root;

        private Dictionary<string, object> _items;

        private QuickSaveReader(string root)
        {
            _root = root;
        }

        public static QuickSaveReader Create(string root)
        {
            QuickSaveReader saveManagerReader = new QuickSaveReader(root);
            saveManagerReader.Open();
            return saveManagerReader;
        }

        public T Read<T>(string key)
        {
            if (!_items.ContainsKey(key))
            {
                throw new ArgumentException("Key does not exists");
            }

            try
            {
                string propertyJson = JsonSerialiser.Serialise(_items[key]);

                return JsonSerialiser.Deserialise<T>(propertyJson);
            }
            catch
            {
                throw new InvalidOperationException("Unable to deserialise data");
            }
        }

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

        public bool KeyExists(string key)
        {
            return _items.ContainsKey(key);
        }

        public IEnumerable<string> GetAllKeys()
        {
            return _items.Keys.ToList();
        }

        private void Open()
        {
            string fileJson = FileAccess.Load(_root);

            if (string.IsNullOrEmpty(fileJson))
            {
                throw new ArgumentException("Root does not exist");
            }

            try
            {
                _items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson);

                if(_items != null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load data", e);
            }
        }
    }
}