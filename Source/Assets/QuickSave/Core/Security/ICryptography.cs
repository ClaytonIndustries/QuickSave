////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.QuickSave.Core.Security
{
    public interface ICryptography
    {
        string Encrypt(string value, SecurityMode securityMode, string password);
        string Decrypt(string value, SecurityMode securityMode, string password);
    }
}