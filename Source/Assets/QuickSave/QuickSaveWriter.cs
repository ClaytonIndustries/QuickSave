﻿////////////////////////////////////////////////////////////////////////////////
//
// @module Quick Save for Unity3D
// @author Michael Clayton
// @support clayton.inds+support@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Threading.Tasks;
using CI.QuickSave.Core.Serialisers;

namespace CI.QuickSave
{
    public class QuickSaveWriter : QuickSaveBase
    {
        private QuickSaveWriter(string root, QuickSaveSettings settings)
        {
            _root = root;
            _settings = settings;
        }

        /// <summary>
        /// Creates a QuickSaveWriter on the specified root
        /// </summary>
        /// <param name="root">The root to write to</param>
        /// <returns>A QuickSaveWriter instance</returns>
        public static QuickSaveWriter Create(string root)
        {
            return Create(root, new QuickSaveSettings());
        }

        /// <summary>
        /// Creates a QuickSaveWriter on the specified root using the specified settings
        /// </summary>
        /// <param name="root">The root to write to</param>
        /// <param name="settings">Settings</param>
        /// <returns>A QuickSaveWriter instance</returns>
        public static QuickSaveWriter Create(string root, QuickSaveSettings settings)
        {
            QuickSaveWriter quickSaveWriter = new QuickSaveWriter(root, settings);
            quickSaveWriter.Load(true);
            return quickSaveWriter;
        }

        /// <summary>
        /// Writes an object to the specified key - you must called commit to write the data to file
        /// </summary>
        /// <typeparam name="T">The type of object to write</typeparam>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <returns>The QuickSaveWriter</returns>
        public QuickSaveWriter Write<T>(string key, T value)
        {
            if (Exists(key))
            {
                _items.Remove(key);
            }

            _items.Add(key, JsonSerialiser.SerialiseKey(value));

            return this;
        }

        /// <summary>
        /// Writes an object to the specified key asynchronously - you must called commit to write the data to file.
        /// </summary>
        /// <typeparam name="T">The type of object to write.</typeparam>
        /// <param name="key">The key this object will be saved under.</param>
        /// <param name="value">The object to save.</param>
        public async Task WriteAsync<T>(string key, T value)
        {
            Task write = new Task(() =>
            {
                Write<T>(key, value);
            });

            write.Start();
            await write;
            write.Dispose();
        }

        /// <summary>
        /// Deletes the specified key if it exists
        /// </summary>
        /// <param name="key">The key to delete</param>
        public void Delete(string key)
        {
            if (Exists(key))
            {
                _items.Remove(key);
            }
        }

        /// <summary>
        /// Commits the changes to file
        /// </summary>
        public void Commit()
        {
            Save();
        }

        /// <summary>
        /// Commits the changes to file asynchronously
        /// </summary>
        public async Task CommitAsync()
        {
            Task commit = new Task(Save);

            commit.Start();
            await commit;
            commit.Dispose();
        }

        /// <summary>
        /// Attempts to commit the changes to file
        /// </summary>
        /// <returns>Was the commit successful</returns>
        public bool TryCommit()
        {
            try
            {
                Save();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to commit the changes to file asynchronously
        /// </summary>
        /// <returns>Was the commit successful</returns>
        public async Task<bool> TryCommitAsync()
        {
            bool result = false;
            Task commit = new Task(() =>
            {
                try
                {
                    Save();

                    result = true;
                }
                catch { }
            });

            commit.Start();
            await commit;
            commit.Dispose();

            return result;
        }
    }
}
