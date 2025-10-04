using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenLibrary.EncryptDecrypt.Hmac;


namespace TokenLibrary.EncryptDecrypt.Hmac
{
    public static class Hashing
    {
        public static string? GetHash(string key, string text)
        {
            string? hash = null;

            if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(text))
            {
                hash = HmacHash.Encrypt(key, text);
            }
            return hash;
        }
    }
}
