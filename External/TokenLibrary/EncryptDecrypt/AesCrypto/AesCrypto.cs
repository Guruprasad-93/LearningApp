using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TokenLibrary.EncryptDecrypt.AesCrypto
{
    public static class EncryptDecryptAesCrypto
    {
        public static string Encrypt(string stringToEncrypt, string key)
        {
            if (!string.IsNullOrEmpty(stringToEncrypt) && !string.IsNullOrEmpty(key))
            {
                return AesCryptoGraphyCore.Encrypt(stringToEncrypt, key);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string Decrypt(string stringToEncrypt, string key)
        {
            if (!string.IsNullOrEmpty(stringToEncrypt) && !string.IsNullOrEmpty(key))
            {
                return AesCryptoGraphyCore.Decrypt(stringToEncrypt, key, false);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
