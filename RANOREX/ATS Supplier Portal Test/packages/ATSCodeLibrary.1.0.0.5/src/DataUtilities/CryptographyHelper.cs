using ATS.CodeLibrary.svwebservice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.Cryptography
{
    /// <summary>
    /// Provides methods for encrypting text and decrypting text and files
    /// </summary>
    public class CryptographyHelper
    {
        private static byte[] Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

        /// <summary>
        /// Decrypt the input text
        /// </summary>
        /// <param name="inputText">Text to decrpyt</param>
        /// <returns>Decrypted string</returns>
        public static string DecryptString(string inputText)
        {
            string plainText = "";

            RijndaelManaged RMCrypto = new RijndaelManaged();

            RMCrypto.KeySize = 128;
            RMCrypto.BlockSize = 128;
            RMCrypto.Key = Key;
            RMCrypto.IV = Key;
            try
            {
                Byte[] input = Convert.FromBase64String(inputText);
                Byte[] output = RMCrypto.CreateDecryptor().TransformFinalBlock(input, 0,
                    input.Length);
                return Encoding.UTF8.GetString(output);
            }
            catch
            {
                plainText = inputText;
            }
            return plainText;
        }

        /// <summary>
        /// Encrypt the input text
        /// </summary>
        /// <param name="inputText">Text to encrypt</param>
        /// <returns>Encrypted string</returns>
        public static string EncryptString(string inputText)
        {
            if (inputText == null)
                return null;

            if (string.IsNullOrWhiteSpace(inputText))
                return inputText;

            string encryptedText = "";

            RijndaelManaged RMCrypto = new RijndaelManaged();

            RMCrypto.KeySize = 128;
            RMCrypto.BlockSize = 128;
            RMCrypto.Key = Key;
            RMCrypto.IV = Key;
            try
            {
                Byte[] input = Encoding.UTF8.GetBytes(inputText);
                Byte[] output = RMCrypto.CreateEncryptor().TransformFinalBlock(input, 0,
                    input.Length);
                return Convert.ToBase64String(output);
            }
            catch
            {
                return encryptedText;
            }
        }

        /// <summary>
        /// Decrypt an entire file
        /// </summary>
        /// <param name="filePath">Full path of the encrypted file</param>
        /// <returns>Decrypted stream</returns>
        public static CryptoStream DecryptFile(string filePath)
        {

            RijndaelManaged RMCrypto = new RijndaelManaged();
            RMCrypto.Key = Key;
            RMCrypto.IV = Key;

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(filePath,
                FileMode.Open,
                FileAccess.Read);

            ICryptoTransform desdecrypt = RMCrypto.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                desdecrypt,
                CryptoStreamMode.Read);

            //descrypted stream
            return cryptostreamDecr;

        }

    }
}
