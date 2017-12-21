////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.QuickSave.Core.Security
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