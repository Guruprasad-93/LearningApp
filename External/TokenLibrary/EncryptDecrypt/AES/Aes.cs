using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TokenLibrary.EncryptDecrypt.AES
{
    public static class EncryptDecryptAes
    {
        public static string? StrEncryptionKey { get; set; }

        public static string DecryptStringAES(string cipherText)
        {
            string decryptedfromJavascript = string.Empty;

            if (!string.IsNullOrEmpty(StrEncryptionKey))
            {
                var keybytes = Encoding.UTF8.GetBytes(StrEncryptionKey);

                var iv = Encoding.UTF8.GetBytes(StrEncryptionKey);

                //var keybytes = Convert.FromBase64String(StrEncryptionKey);
                //var iv = Convert.FromBase64String(StrEncryptionKey);



                var encrypted = Convert.FromBase64String(cipherText);

                decryptedfromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            }
            return decryptedfromJavascript;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if(cipherText == null && cipherText?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }
            if (key == null && key?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null && iv?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            string plaintext = string.Empty;
            // Create an RijndaelManaged object
            using var rijAlg = Aes.Create("AesManaged");
            if(rijAlg != null)
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decryptor to perform the stream transform

                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    if (cipherText != null)
                    {
                        using var msDecrypt = new MemoryStream(cipherText);
                        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                        using var srDecrypt = new StreamReader(csDecrypt);

                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
                catch
                {
                    plaintext = "KeyError";
                }
            }
            return plaintext;
        }

        public static string EncryptStringAES(string plainText)
        {
            byte[] encryptedfromJavascript = Array.Empty<byte>();

            if (!string.IsNullOrEmpty(StrEncryptionKey))
            {
                var keybytes = Encoding.UTF8.GetBytes(StrEncryptionKey);

                var iv = Encoding.UTF8.GetBytes(StrEncryptionKey);

                //var keybytes = Convert.FromBase64String(StrEncryptionKey);
                //var iv = Convert.FromBase64String(StrEncryptionKey);



                encryptedfromJavascript = EncryptStringToBytes(plainText, keybytes, iv);
            }
            return Convert.ToBase64String(encryptedfromJavascript);
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null && plainText?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(plainText));
            }
            if (key == null && key?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null && iv?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            byte[] encrypted = Array.Empty<byte>();

            using var rijAlg = Aes.Create("AesManaged");
            if (rijAlg != null)
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                try
                {

                    using MemoryStream msEncrypt = new();
                    using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                    using (StreamWriter swEncrypt = new(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    encrypted = msEncrypt.ToArray();
                }
                catch
                {
                    encrypted = Array.Empty<byte>();
                }
            }

            return encrypted;
        }
    }
}
