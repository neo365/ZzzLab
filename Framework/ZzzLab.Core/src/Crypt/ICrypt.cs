using System.IO;
using System.Text;

namespace ZzzLab.Crypt
{
    public interface ICrypt
    {
        string Algorithm { get; }

        #region Encrypt

        string EncryptToString(Stream stream);

        string EncryptToString(byte[] bytes);

        string EncryptToString(string s, Encoding encoding = null);

        byte[] EncryptToBytes(Stream stream);

        byte[] EncryptToBytes(byte[] bytes);

        byte[] EncryptToBytes(string s, Encoding encoding = null);

        #endregion Encrypt

        #region Decrypt

        string DecryptToString(Stream stream);

        string DecryptToString(byte[] bytes);

        string DecryptToString(string s);

        byte[] DecryptToBytes(Stream stream);

        byte[] DecryptToBytes(byte[] bytes);

        byte[] DecryptToBytes(string s);

        #endregion Decrypt

        #region URL Safe

        string EncryptUrlSafe(string s);

        string DecryptUrlSafe(string s);

        #endregion URL Safe
    }
}