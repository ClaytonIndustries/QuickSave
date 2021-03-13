using UnityEngine;

namespace CI.QuickSave
{
    public static class QuickSaveGlobalSettings
    {
        /// <summary>
        /// The path to save data to - defaults to Application.persistentDataPath
        /// </summary>
        public static string StorageLocation { get; set; } = Application.persistentDataPath;
    }
}