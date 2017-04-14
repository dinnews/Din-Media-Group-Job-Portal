using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Din_Media_Group_Job_Portal.Models;
namespace Din_Media_Group_Job_Portal.DBService
{
    public class PasswordEncryption
    {
        #region passwordRelatedChanges
        private string key1 = ProjectConstants.passwordEncryptionKey1;
        private string key2 = ProjectConstants.passwordEncryptionKey2;
        //private const string key1 = "aaaaaaaaaaaaaaaa";
        //private const string key2 = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string EncryptString(string ClearText)
        {
            byte[] clearTextBytes = Encoding.UTF8.GetBytes(ClearText);
            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();
            MemoryStream ms = new MemoryStream();

            byte[] rgbIV = Encoding.ASCII.GetBytes(key1);
            byte[] key = Encoding.ASCII.GetBytes(key2);
            CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);
            cs.Write(clearTextBytes, 0, clearTextBytes.Length);
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        private string DecryptString(string EncryptText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(EncryptText);
            MemoryStream ms = new MemoryStream();
            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();



            byte[] rgbIV = Encoding.ASCII.GetBytes(key1);
            byte[] key = Encoding.ASCII.GetBytes(key2);
            CryptoStream cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);
            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public string encryptPassword(string instring)
        {
            return EncryptString(EncryptString(instring));
        }

        public string decryptPassword(string outstring)
        {
            return DecryptString(DecryptString(outstring));
        }

        #endregion

    }
}