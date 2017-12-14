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
    }
}