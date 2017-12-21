////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if !NETFX_CORE
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CI.QuickSave.Core.Security
{
    public class CryptographyMono : ICryptography
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

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                byte[] data = Encoding.UTF8.GetBytes(value);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string AesDecrypt(string encryptionKey, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            using (Aes decryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                decryptor.Key = pdb.GetBytes(32);
                decryptor.IV = pdb.GetBytes(16);

                byte[] data = Convert.FromBase64String(value);

                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
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
#endif