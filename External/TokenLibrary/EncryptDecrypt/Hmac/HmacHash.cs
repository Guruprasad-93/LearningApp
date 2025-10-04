using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TokenLibrary.EncryptDecrypt.Hmac
{
    public static class HmacHash
    {
        public static string Encrypt1(string key, string text)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var textBytes = Encoding.UTF8.GetBytes(text);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(textBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public static string Encrypt(string Key, string password)
        {
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            HMACSHA1 hMACSHA = new HMACSHA1(uTF8Encoding.GetBytes(Key));
            byte[] bytes_Input = hMACSHA.ComputeHash(uTF8Encoding.GetBytes(password.Trim().ToString()));
            return Bytes_To_String2(bytes_Input);
        }

        private static string Bytes_To_String2(byte[] bytes_Input)
        {
            StringBuilder stringBuilder = new StringBuilder(bytes_Input.Length * 2);
            foreach (byte b in bytes_Input)
            {
                stringBuilder.Append(b.ToString("X02"));
            }

            return stringBuilder.ToString();
        }
    }
}
