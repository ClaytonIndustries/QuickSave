
namespace CI.QuickSave.Core
{
    public interface ICryptography
    {
        string Encrypt(string value, SecurityMode securityMode, string password);
        string Decrypt(string value, SecurityMode securityMode, string password);
    }
}