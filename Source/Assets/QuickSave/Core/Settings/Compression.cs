using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using CICompressionMode = CI.QuickSave.CompressionMode;

namespace CI.QuickSave.Core.Settings
{
    public class Compression
    {
        public static string Compress(string value, CICompressionMode compressionMode)
        {
            if (compressionMode == CICompressionMode.None)
            {
                return value;
            }

            var bytes = Encoding.UTF8.GetBytes(value);

            using (var streamIn = new MemoryStream(bytes))
            using (var streamOut = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(streamOut, System.IO.Compression.CompressionMode.Compress))
                {
                    streamIn.CopyTo(compressionStream);
                }

                return Convert.ToBase64String(streamOut.ToArray());
            }
        }

        public static string Decompress(string value, CICompressionMode compressionMode)
        {
            if (compressionMode == CICompressionMode.None)
            {
                return value;
            }

            var bytes = Convert.FromBase64String(value);

            using (var streamIn = new MemoryStream(bytes))
            using (var streamOut = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(streamIn, System.IO.Compression.CompressionMode.Decompress))
                {
                    compressionStream.CopyTo(streamOut);
                }

                return Encoding.UTF8.GetString(streamOut.ToArray());
            }
        }
    }
}