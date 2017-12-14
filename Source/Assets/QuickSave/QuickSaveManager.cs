using System;
using System.Collections.Generic;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public static class QuickSaveManager
    {
        public static void Save<T>(string root, string key, T value, bool replace)
        {
            string fileJson = FileAccess.Load(root);

            Dictionary<string, object> items = null;

            if (string.IsNullOrEmpty(fileJson))
            {
                items = new Dictionary<string, object>();
            }
            else
            {
                try
                {
                    items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson) ?? new Dictionary<string, object>();
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Unable to deserialise data", e);
                }

                if (items.ContainsKey(key))
                {
                    if (replace)
                    {
                        items.Remove(key);
                    }
                    else
                    {
                        throw new ArgumentException("Key already exists");
                    }
                }
            }

            items.Add(key, value);

            try
            {
                string jsonToSave = JsonSerialiser.Serialise(items);

                FileAccess.Save(root, jsonToSave);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to save data", e);
            }
        }

        public static T Load<T>(string root, string key)
        {
            string fileJson = FileAccess.Load(root);

            if (string.IsNullOrEmpty(fileJson))
            {
                throw new ArgumentException("Root does not exist");
            }

            Dictionary<string, object> items = null;

            try
            {
                items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson) ?? new Dictionary<string, object>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load data", e);
            }

            if (!items.ContainsKey(key))
            {
                throw new ArgumentException("Key does not exists");
            }

            try
            {
                string propertyJson = JsonSerialiser.Serialise(items[key]);

                return JsonSerialiser.Deserialise<T>(propertyJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to deserialise data", e);
            }
        }

        public static bool TryLoad<T>(string root, string key, out T result)
        {
            result = default(T);

            string fileJson = FileAccess.Load(root);

            if (string.IsNullOrEmpty(fileJson))
            {
                return false;
            }

            Dictionary<string, object> items = null;

            try
            {
                items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson) ?? new Dictionary<string, object>();
            }
            catch
            {
                return false;
            }

            if (!items.ContainsKey(key))
            {
                return false;
            }

            try
            {
                string propertyJson = JsonSerialiser.Serialise(items[key]);

                result = JsonSerialiser.Deserialise<T>(propertyJson);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Delete(string root)
        {
            FileAccess.Delete(root);
        }

        public static void Delete(string root, string key)
        {
            try
            {
                string fileJson = FileAccess.Load(root);

                if (string.IsNullOrEmpty(fileJson))
                {
                    return;
                }

                Dictionary<string, object> items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson);

                if (items.ContainsKey(key))
                {
                    items.Remove(key);

                    string jsonToSave = JsonSerialiser.Serialise(items);

                    FileAccess.Save(root, jsonToSave);
                }
            }
            catch
            {
            }
        }

        public static bool RootExists(string root)
        {
            return FileAccess.FileExists(root);
        }

        public static bool KeyExists(string root, string key)
        {
            string fileJson = FileAccess.Load(root);

            if (string.IsNullOrEmpty(fileJson))
            {
                throw new ArgumentException("Root does not exist");
            }

            Dictionary<string, object> items = null;

            try
            {
                items = JsonSerialiser.Deserialise<Dictionary<string, object>>(fileJson) ?? new Dictionary<string, object>();
            }
            catch
            {
                throw new Exception("Unable to load data");
            }

            return items.ContainsKey(key);
        }
    }
}