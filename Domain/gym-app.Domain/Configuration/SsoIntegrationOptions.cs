using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Configuration
{
    public class SsoIntegrationOptions
    {
        public SsoJwtOptions ssoJwtOptions { get; set; }
        public SsoProviderType ssoProviderType { get; set; }
    }

    public class SsoJwtOptions
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }

    public enum SsoProviderType
    {
        Jwt = 1,
        Lti = 2
    }
}
