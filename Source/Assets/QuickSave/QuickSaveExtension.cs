using System.Threading.Tasks;

namespace CI.QuickSave
{
    /// <summary>
    /// <para>Extension asynchronous methods for the QuickSave asset on the
    /// Unity Asset Store: https://assetstore.unity.com/packages/tools/integration/quick-save-107676.</para>
    /// <para>Just drag this file into anywhere in your project and you'll be able to call these methods.</para>
    /// There is no "TryReadAsync" method as "out" parameters are not permitted in async methods, just use a try-catch block instead.
    /// </summary>
    public static class QuickSaveExtension
    {
        /// <summary>
        /// Writes an object to the specified key asynchronously - you must called commit to write the data to file.
        /// </summary>
        /// <typeparam name="T">The type of object to write.</typeparam>
        /// <param name="key">The key this object will be saved under.</param>
        /// <param name="value">The object to save.</param>
        public static async Task WriteAsync<T>(this QuickSaveWriter writer, string key, T value)
        {
            Task write = new Task(() =>
            {
                writer.Write<T>(key, value);
            });

            write.Start();
            await write;
            write.Dispose();
        }

        /// <summary>
        /// Reads an object under the specified key asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <returns>The object that was loaded.</returns>
        public static async Task<T> ReadAsync<T>(this QuickSaveReader reader, string key)
        {
            T result = default;
            Task read = new Task(() =>
            {
                result = reader.Read<T>(key);
            });

            read.Start();
            await read;
            read.Dispose();

            return result;
        }
    }
}
