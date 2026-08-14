using Vendor_Portal.App_Code.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Vendor_Portal.App_Code.BLL
{
    public class bllLogin
    {
        DALLogin dalLogin = new DALLogin();
        public int ValidateUser(string Username, string Password)
        {
            return dalLogin.ValidateUser(Username, Password);
        }

        public DataTable GetUserById(int EmployeeID, string Username, string Password)
        {
            return dalLogin.GetUserById(EmployeeID, Username, Password);
        }

        public DataTable BlockUserLogin(string Code)
        {
            return dalLogin.BlockUserLogin(Code);
        }
        public DataTable GetUserInformation(int EmployeeID)
        {
            return dalLogin.GetUserInformation(EmployeeID);
        }

        public DataTable GetUserMfaSetting(int EmployeeID)
        {
            return dalLogin.GetUserMfaSetting(EmployeeID);
        }

        public void SaveUserMfaSetting(int EmployeeID, string UserName, string ProtectedSecret)
        {
            dalLogin.SaveUserMfaSetting(EmployeeID, UserName, ProtectedSecret);
        }

        public void MarkUserMfaVerified(int EmployeeID)
        {
            dalLogin.MarkUserMfaVerified(EmployeeID);
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
