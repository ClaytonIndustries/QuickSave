////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Serialisers;
using CI.QuickSave.Core.Storage;
using CI.QuickSave.Core.Util;
using Newtonsoft.Json.Linq;

namespace CI.QuickSave
{
    public abstract class QuickSaveBase
    {
        protected JObject _items;
        protected string _root;
        protected QuickSaveSettings _settings;

        /// <summary>
        /// Determines whether the specified root exists
        /// </summary>
        /// <param name="root">The root to check</param>
        /// <returns>Does the root exist</returns>
        public static bool RootExists(string root) => FileAccess.Exists(FileAccess.GetPathFromBase(root));

        /// <summary>
        /// Deletes the specified root if it exists
        /// </summary>
        /// <param name="root">The root to delete</param>
        public static void DeleteRoot(string root) => FileAccess.Delete(FileAccess.GetPathFromBase(root));

        /// <summary>
        /// Determines if the specified key exists
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>Does the key exist</returns>
        public bool Exists(string key) => _items[key] != null;

        /// <summary>
        /// Gets the names of all the keys under this root
        /// </summary>
        /// <returns>A collection of key names</returns>
        public IEnumerable<string> GetAllKeys() => _items.Properties().Select(x => x.Name).ToList();

        protected void Load(bool rootMightNotExist)
        {
            FileAccess.CreateRootFolder();
            var json = FileAccess.LoadString(FileAccess.GetPathFromBase(_root));

            if (string.IsNullOrEmpty(json))
            {
                if (rootMightNotExist)
                {
                    _items = new JObject();

                    return;
                }

                throw new QuickSaveException("Root does not exist");
            }

            json = DataService.PrepareLoadedData(json, _settings);

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

            json = DataService.PrepareSaveData(json, _settings);

            FileAccess.CreateRootFolder();
            if (!FileAccess.SaveString(FileAccess.GetPathFromBase(_root), json))
            {
                throw new QuickSaveException("Failed to write to file");
            }
        }
    }
}