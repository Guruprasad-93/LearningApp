using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.User
{
    public class UserDetailsModel
    {
        public long UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateOnly? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long CompanyId { get; set; }

        public long RoleId { get; set; }

        public IFormFile? PhotoPath { get; set; }
    }

    public class UserDetails
    {
        public long UserId { get; set; }
        public string? PhotoPath { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNo { get; set; }
        public string? gender { get; set; }
        public string? MemberShip { get; set; }
    }

    public class UserProfile
    {
        public UserProfile()
        {
            dietplans = new List<Dietplans>();
        }

        public long UserId { get; set; }
        public string? PhotoPath { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNo { get; set; }
        public string? Trainer { get; set; }
        public string? Speciality { get; set; }
        public string? gender { get; set; }
        public int? Star { get; set; }
        public string? Review { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<Dietplans> dietplans { get; set; }
    }

    public class Dietplans
    {
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }
}
