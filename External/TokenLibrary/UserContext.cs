using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TokenLibrary
{
    public sealed class TokenUserContext
    {
        public long UserId { get; set; }
        public string? EmailId { get; set; }
        public string? LoginId { get; set; }
        public UserRole? CurrentRole { get; set; }
        public string? SessionId { get; set; }
        public UserTimeZone? userTimeZone { get; set; }


    }

    public sealed class UserRole
    {
        public long RoleId { get; set; }
        public string? RoleCode { get; set; }
        public string? RoleName { get; set; }
        public UserRoleType? RoleType { get; set; }
    }

    public enum UserRoleType
    {
        NONE = 0,
        SuperAdmin = 1,
        Admin = 2,
        User = 3
    }

    public sealed class UserTimeZone
    {
        public string? TimeZoneCode { get; set; }
        public string? TimeZoneName { get; set; }
        public int BaseUTCOffsetInMin { get; set; }
    }
}
