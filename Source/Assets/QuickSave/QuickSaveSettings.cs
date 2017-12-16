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
        public SecurityMode SecurityMode { get; set; }
        public string Password { get; set; }

        public QuickSaveSettings()
        {
            SecurityMode = SecurityMode.None;
        }
    }
}