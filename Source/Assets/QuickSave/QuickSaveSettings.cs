////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.QuickSave
{
    public class QuickSaveSettings
    {
        /// <summary>
        /// The type of encryption to use on the data
        /// </summary>
        public SecurityMode SecurityMode { get; set; }

        /// <summary>
        /// The type of compression to use on the data
        /// </summary>
        public CompressionMode CompressionMode { get; set; }

        /// <summary>
        /// If aes is selected as the security mode specify a password to use as the encryption key
        /// </summary>
        public string Password { get; set; }

        public QuickSaveSettings()
        {
            SecurityMode = SecurityMode.None;
            CompressionMode = CompressionMode.None;
        }
    }
}