////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using CI.QuickSave.Core;

namespace CI.QuickSave
{
    public class QuickSaveRaw
    {
        /// <summary>
        /// Saves an object to a file
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        public static void Save<T>(string root, T value)
        {
            Save(root, value, new QuickSaveSettings());
        }

        /// <summary>
        /// Saves an object to a file using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <param name="settings">Settings</param>
        public static void Save<T>(string root, T value, QuickSaveSettings settings)
        {
            string jsonToSave;

            try
            {
                jsonToSave = JsonSerialiser.Serialise(TypeHelper.ReplaceIfUnityType(value));
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Json serialisation failed", e);
            }

            string encryptedJson;

            try
            {
                encryptedJson = Cryptography.Encrypt(jsonToSave, settings.SecurityMode, settings.Password);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Encryption failed", e);
            }

            if(!FileAccess.Save(root, encryptedJson))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }

        /// <summary>
        /// Loads an object from a file
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <returns>The object that was loaded</returns>
        public static T Load<T>(string root)
        {
            return Load<T>(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Loads an object from a file using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <param name="settings">Settings</param>
        /// <returns>The object that was loaded</returns>
        public static T Load<T>(string root, QuickSaveSettings settings)
        {
            string fileJson = FileAccess.Load(root);

            if(string.IsNullOrEmpty(fileJson))
            {
                throw new QuickSaveException("File either does not exist or is empty");
            }

            string decryptedJson;

            try
            {
                decryptedJson = Cryptography.Decrypt(fileJson, settings.SecurityMode, settings.Password);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Decryption failed", e);
            }

            try
            {
                return JsonSerialiser.Deserialise<T>(decryptedJson);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Failed to deserialise json", e);
            }
        }
    }
}