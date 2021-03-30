////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CI.QuickSave.Core.Settings
{
    public static class Cryptography
    {
        public static string Encrypt(string value, SecurityMode securityMode, string password)
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

        public static string Decrypt(string value, SecurityMode securityMode, string password)
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

        private static string AesEncrypt(string encryptionKey, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            using (var encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                byte[] bytes = Encoding.UTF8.GetBytes(value);

                using (var streamIn = new MemoryStream(bytes))
                using (var streamOut = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(streamOut, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        streamIn.CopyTo(cryptoStream);
                    }

                    return Convert.ToBase64String(streamOut.ToArray());
                }
            }
        }

        private static string AesDecrypt(string encryptionKey, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            using (var decryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                decryptor.Key = pdb.GetBytes(32);
                decryptor.IV = pdb.GetBytes(16);

                byte[] bytes = Convert.FromBase64String(value);

                using (var streamIn = new MemoryStream(bytes))
                using (var streamOut = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(streamIn, decryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(streamOut);
                    }

                    return Encoding.UTF8.GetString(streamOut.ToArray());
                }
            }
        }

        private static string Base64Encode(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        private static string Base64Decode(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}