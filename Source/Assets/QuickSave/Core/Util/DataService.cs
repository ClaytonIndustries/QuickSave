using System;

namespace CI.QuickSave.Core.Util
{
    public static class DataService
    {
        public static string PrepareSaveData(string data, QuickSaveSettings settings)
        {
            try
            {
                data = Compression.Compress(data, settings.CompressionMode);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Compression failed", e);
            }

            // Gzip outputs base64 anyway so no need to do it twice
            if (settings.CompressionMode != CompressionMode.Gzip || settings.SecurityMode != SecurityMode.Base64)
            {
                try
                {
                    data = Cryptography.Encrypt(data, settings.SecurityMode, settings.Password);
                }
                catch (Exception e)
                {
                    throw new QuickSaveException("Encryption failed", e);
                }
            }

            return data;
        }

        public static string PrepareLoadedData(string data, QuickSaveSettings settings)
        {
            // Gzip parses base64 anyway so no need to do it twice
            if (settings.CompressionMode != CompressionMode.Gzip || settings.SecurityMode != SecurityMode.Base64)
            {
                try
                {
                    data = Cryptography.Decrypt(data, settings.SecurityMode, settings.Password);
                }
                catch (Exception e)
                {
                    throw new QuickSaveException("Decryption failed", e);
                }
            }

            try
            {
                data = Compression.Decompress(data, settings.CompressionMode);
            }
            catch (Exception e)
            {
                throw new QuickSaveException("Decompression failed", e);
            }

            return data;
        }
    }
}