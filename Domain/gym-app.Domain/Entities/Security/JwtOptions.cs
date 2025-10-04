using gym_app.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace gym_app.Domain.Entities.Security
{
    public class JwtOptions
    {
        public JwtOptions(IConfiguration config)
        {
            ValidAudience = config.GetValue("JwtOptions:Audience", "https://localhost:44378");
            ValidIssuer = config.GetValue("JwtOptions:Issuer", "https://localhost:4200");
           // Secret = DecryptDomain.DecryptAes(config.GetValue("JwtOptions:Secret","RR/SrNWrcilTG2Iyn69hlad18gekQFFfRHYPnbuQAuzVPSGV1iXqjVMYQgx4Bk/EzttuNdKgxrKIRyt1E3sLSQ=="));
            Secret = DecryptDomain.DecryptAes(config.GetValue("JwtOptions:Secret", "mjt5YyzDRLVjzqj1JiZPQ64bb9DK2EiRR09y2M0ipIjqGAephTW8f6T4q+R3a8xn"));
            TokenValidityInMinutes = config.GetValue("JwtOptions:TokenValidityInMinutes", 5);
            RefreshTokenValidityInMinutes = config.GetValue("JwtOptions:ExpiryInMinutes", 20);
           
        }

        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInMinutes { get; set; }
    }
}
