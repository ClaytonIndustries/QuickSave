////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.QuickSave.Core
{
    public static class JsonSerialiser
    {
#if !NETFX_CORE
        private static IJsonSerialiser _serialiser = new JsonSerialiserMono();
#else
        private static IJsonSerialiser _serialiser = new JsonSerialiserUWP();
#endif

        public static string Serialise<T>(T value)
        {
            return _serialiser.Serialise(value);
        }

        public static T Deserialise<T>(string json)
        {
            return _serialiser.Deserialise<T>(json);
        }
    }
}
