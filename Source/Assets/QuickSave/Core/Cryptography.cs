
namespace CI.QuickSave.Core
{
    public static class Cryptography
    {
#if !NETFX_CORE
        private static ICryptography _cryptography = new CryptographyMono();
#else
        private static ICryptography _cryptography = new CryptographyUWP();
#endif

        public static string Encrypt(string value, SecurityMode securityMode, string password)
        {
            return _cryptography.Encrypt(value, securityMode, password);
        }

        public static string Decrypt(string value, SecurityMode securityMode, string password)
        {
            return _cryptography.Decrypt(value, securityMode, password);
        }
    }
}