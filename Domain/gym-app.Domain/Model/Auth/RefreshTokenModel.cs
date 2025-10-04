using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.Auth
{
    public class UserLoginTokenModel
    {
        public long TokenId { get; set; }

        public long? UserId { get; set; }

        public string? UserName { get; set; }
        public string? sessionId { get; set; }
        public string? IpAddress { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? Expires { get; set; }

        public bool IsExpired { get; set; }

        public bool IsActive { get; set; }

        public string? JwtToken { get; set; }

        public long? RevokedBy { get; set; }

        public DateTime? Revoked { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long CompanyId { get; set; }


    }
}
