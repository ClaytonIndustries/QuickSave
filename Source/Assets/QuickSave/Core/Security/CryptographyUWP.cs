////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace CI.QuickSave.Core.Security
{
    public class CryptographyUWP : ICryptography
    {
        public string Encrypt(string value, SecurityMode securityMode, string password)
        {
            switch (securityMode)
            {
                case SecurityMode.Aes:
                    return AesEncrypt(password, value);
                case SecurityMode.Base64:
                    return Base64Encode(value);
                default:
                    return value;
            }
        }

        public string Decrypt(string value, SecurityMode securityMode, string password)
        {
            switch (securityMode)
            {
                case SecurityMode.Aes:
                    return AesDecrypt(password, value);
                case SecurityMode.Base64:
                    return Base64Decode(value);
                default:
                    return value;
            }
        }

        private string AesEncrypt(string encryptionKey, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            IBuffer passwordBuffer = CryptographicBuffer.ConvertStringToBinary(encryptionKey, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.CreateFromByteArray(new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            IBuffer contentBuffer = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

            KeyDerivationAlgorithmProvider keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha1); 
            KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(passwordBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(passwordBuffer);

            IBuffer ivBuffer = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

            CryptographicKey aesKey = aes.CreateSymmetricKey(keyMaterial);

            return CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(aesKey, contentBuffer, ivBuffer));
        }

        private string AesDecrypt(string encryptionKey, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            IBuffer passwordBuffer = CryptographicBuffer.ConvertStringToBinary(encryptionKey, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.CreateFromByteArray(new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            KeyDerivationAlgorithmProvider keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha1);
            KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(passwordBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(passwordBuffer);

            IBuffer ivBuffer = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

            CryptographicKey aesKey = aes.CreateSymmetricKey(keyMaterial);

            IBuffer decryptedContentBuffer = CryptographicEngine.Decrypt(aesKey, CryptographicBuffer.DecodeFromBase64String(value), ivBuffer);

            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedContentBuffer);
        }

        private static string Base64Encode(string value)
        {
            return CryptographicBuffer.EncodeToBase64String(CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8));
        }

        private static string Base64Decode(string value)
        {
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, CryptographicBuffer.DecodeFromBase64String(value));
        }
    }
}
#endif