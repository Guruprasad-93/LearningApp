using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenLibrary.JwtToken
{
    public interface IAppOptions
    {
        public string EncryptionAlgorithmkey { get; set; }
    }

    public class TokenJwtOptions : IAppOptions
    {
        public TokenJwtOptions()
        {
            ValidAudience = string.Empty;
            ValidIssuer = string.Empty;
            Secret = string.Empty;
            TokenValidityInMinutes = 0;
            RefreshTokenValidityInMinutes = 0;
            EncryptionAlgorithmkey = string.Empty;
        }

        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInMinutes { get; set; }
        public string EncryptionAlgorithmkey { get; set; }
    }
}
