using System;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public class QuickSaveRaw
    {
        public static void Save<T>(string root, T value)
        {
            try
            {
                string jsonToSave = JsonSerialiser.Serialise(value);

                FileAccess.Save(root, jsonToSave);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to save data", e);
            }
        }

        public static void Save<T>(string root, T value, QuickSaveSettings settings)
        {
            try
            {
                string jsonToSave = JsonSerialiser.Serialise(value);

                string encryptedJson = Cryptography.Encrypt(jsonToSave, settings.SecurityMode, settings.Password);

                FileAccess.Save(root, encryptedJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to save data", e);
            }
        }

        public static T Load<T>(string root)
        {
            try
            {
                string fileJson = FileAccess.Load(root);

                return JsonSerialiser.Deserialise<T>(fileJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load data", e);
            }
        }

        public static T Load<T>(string root, QuickSaveSettings settings)
        {
            try
            {
                string fileJson = FileAccess.Load(root);

                string decryptedJson = Cryptography.Decrypt(fileJson, settings.SecurityMode, settings.Password);

                return JsonSerialiser.Deserialise<T>(decryptedJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load data", e);
            }
        }
    }
}