using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.Auth
{
    public class AuthenticateResponseModel
    {
        public string Token { get; set; }
        // [XssTextValidation]
        public string RefreshToken { get; set; }
        // [XssTextValidation]
        public string Roles { get; set; }
        // [XssTextValidation]
        public string SessionKey { get; set; }
        // [XssTextValidation]
        public double RefreshInterval { get; set; }
        public bool? IsFirstTimeLoggedIn { get; set; }
        public string LoginId { get; set; }
        public long UserId { get; set; }
        public string Status { get; set; }
        public string RefKey { get; set; }
        public string Message { get; set; }

    }
}
