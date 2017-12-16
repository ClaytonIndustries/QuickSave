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
        /// Save an object to a file
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        public static void Save<T>(string root, T value)
        {
            Save(root, value, new QuickSaveSettings());
        }

        /// <summary>
        /// Save an object to a file using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to save</typeparam>
        /// <param name="root">The root this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <param name="settings">Settings</param>
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

        /// <summary>
        /// Load an object from a file
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <returns>The object that was loaded</returns>
        public static T Load<T>(string root)
        {
            return Load<T>(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Load an object from a file using the specified settings
        /// </summary>
        /// <typeparam name="T">The type of object to load</typeparam>
        /// <param name="root">The root this object was saved under</param>
        /// <param name="settings">Settings</param>
        /// <returns>The object that was loaded</returns>
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