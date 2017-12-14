using System.Collections.Generic;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public static class QuickSaveManager
    {
        public static bool TrySave<T>(string root, string key, T value, bool replace)
        {
            return TrySave(root, key, value, replace, new QuickSaveSettings());
        }

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

        public static bool TryLoad<T>(string root, string key, out T result)
        {
            return TryLoad(root, key, new QuickSaveSettings(), out result);
        }

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

        public static void Delete(string root)
        {
            FileAccess.Delete(root);
        }

        public static bool Exists(string root)
        {
            return FileAccess.FileExists(root);
        }
    }
}