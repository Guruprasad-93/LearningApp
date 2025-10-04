using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TokenLibrary.EncryptDecrypt.AesCrypto
{
    public static class AesCryptoGraphyCore
    {
        public static string Encrypt(string plainSourceStringToEncrypt, string encryptKey = "", bool isEncrypted = true)
        {
            string text = "T@yek#MAET7Sec";
            if (!string.IsNullOrEmpty(encryptKey))
            {
                text = ((!isEncrypted) ? encryptKey : Decrypt(encryptKey, text, isEncrypted: false));
            }

            using AesCryptoServiceProvider aesCryptoServiceProvider = GetProvider(Encoding.Default.GetBytes(text));
            byte[] bytes = Encoding.UTF8.GetBytes(plainSourceStringToEncrypt);
            ICryptoTransform transform = aesCryptoServiceProvider.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] inArray = memoryStream.ToArray();
            return Convert.ToBase64String(inArray);
        }

        public static string Decrypt(string base64StringToDecrypt, string encryptKey = "", bool isEncrypted = true)
        {
            string text = "T@yek#MAET7Sec";
            if (!string.IsNullOrEmpty(encryptKey))
            {
                text = ((!isEncrypted) ? encryptKey : Decrypt(encryptKey, text, isEncrypted: false));
            }

            using AesCryptoServiceProvider aesCryptoServiceProvider = GetProvider(Encoding.UTF8.GetBytes(text));
            byte[] array = Convert.FromBase64String(base64StringToDecrypt);
            ICryptoTransform transform = aesCryptoServiceProvider.CreateDecryptor();
            MemoryStream stream = new MemoryStream(array, 0, array.Length);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            return new StreamReader(stream2).ReadToEnd();
        }

        public static bool EncryptFile(string plainFilePath, string encryptedFilePath, string encryptKey = "", bool isEncrypted = true)
        {
            string text = "T@yek#MAET7Sec";
            try
            {
                if (!string.IsNullOrEmpty(encryptKey))
                {
                    if (isEncrypted)
                    {
                        encryptKey = Decrypt(encryptKey, text, isEncrypted: false);
                    }
                    else
                    {
                        text = encryptKey;
                    }
                }

                CryptFile(plainFilePath, encryptedFilePath, text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void CryptFile(string plainFilePath, string encryptedFilePath, string key)
        {
            using AesCryptoServiceProvider aesCryptoServiceProvider = GetProvider(Encoding.Default.GetBytes(key));
            ICryptoTransform transform = aesCryptoServiceProvider.CreateEncryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV);
            byte[] array;
            using (FileStream fileStream = new FileStream(plainFilePath, FileMode.Open, FileAccess.Read))
            {
                array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
            }

            using FileStream stream = new FileStream(plainFilePath, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
        }

        public static bool DecryptFile(string EncryptedFilePath, string decryptedFilePath, string encryptKey = "", bool isEncrypted = true)
        {
            string text = "T@yek#MAET7Sec";
            try
            {
                if (!string.IsNullOrEmpty(encryptKey))
                {
                    if (isEncrypted)
                    {
                        encryptKey = Decrypt(encryptKey, text, isEncrypted: false);
                    }
                    else
                    {
                        text = encryptKey;
                    }
                }

                DCryptFile(EncryptedFilePath, decryptedFilePath, text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void DCryptFile(string EncryptedFilePath, string decryptedFilePath, string key)
        {
            using AesCryptoServiceProvider aesCryptoServiceProvider = GetProvider(Encoding.Default.GetBytes(key));
            ICryptoTransform transform = aesCryptoServiceProvider.CreateDecryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV);
            byte[] array;
            using (FileStream fileStream = new FileStream(EncryptedFilePath, FileMode.Open, FileAccess.Read))
            {
                array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
            }

            using FileStream stream = new FileStream(decryptedFilePath, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider();
            aesCryptoServiceProvider.BlockSize = 128;
            aesCryptoServiceProvider.KeySize = 128;
            aesCryptoServiceProvider.Mode = CipherMode.CBC;
            aesCryptoServiceProvider.Padding = PaddingMode.PKCS7;
            aesCryptoServiceProvider.GenerateIV();
            aesCryptoServiceProvider.IV = new byte[16];
            byte[] key2 = GetKey(key, aesCryptoServiceProvider);
            aesCryptoServiceProvider.Key = key2;
            return aesCryptoServiceProvider;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                list.Add(suggestedKey[i / 8 % suggestedKey.Length]);
            }

            return list.ToArray();
        }
    }
}
