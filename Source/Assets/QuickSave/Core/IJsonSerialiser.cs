
namespace CI.QuickSave.Core
{
    public interface IJsonSerialiser
    {
        string Serialise<T>(T value);
        T Deserialise<T>(string json);
    }
}