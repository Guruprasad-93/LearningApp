using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.Auth
{
    public class AuthenticateRequestModel
    {
        [Required(ErrorMessage = "User Name is Required")]
       // [XssTextValidation]
       public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        // [XssTextValidation]
        public string Password { get; set; }
        public bool? IsFirstTimeLoggedIn { get; set; }
        public string? Loginstatus { get; set; }
        public string? SessionKey { get; set; }


    }
}
