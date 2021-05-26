using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Common.Cryptography
{
    public static class Encrypt_Dycrypt
    {
        public static byte[] getKey
        {
            get
            {
                return ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["EncryptKey"].ToString());
            }
        }

        public static string Encrypt(string originalString)
        {
            return Encrypt(originalString, getKey);
        }

        public static string Encrypt(string originalString, byte[] bytes)
        {
            try
            {
                if (String.IsNullOrEmpty(originalString))
                {
                    throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
                }

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(originalString);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();


                //return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                var Converting = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                Converting = Regex.Replace(Converting, "/", "-");
                Converting = Regex.Replace(Converting, "[+]", "_");
                return Converting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Decrypt(string originalString)
        {
            return Decrypt(originalString, getKey);
        }

        public static string Decrypt(string cryptedString, byte[] bytes)
        {
            try
            {
                if (String.IsNullOrEmpty(cryptedString))
                {
                    throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
                }
                cryptedString = Regex.Replace(cryptedString, "-", "/");
                cryptedString = Regex.Replace(cryptedString, "[ ]", "+");
                cryptedString = Regex.Replace(cryptedString, "_", "+");

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
