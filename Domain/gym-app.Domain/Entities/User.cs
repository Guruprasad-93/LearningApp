using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class User
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

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public long RoleId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CustomerTrainer> CustomerTrainers { get; set; } = new List<CustomerTrainer>();

    public virtual ICollection<Dietplan> Dietplans { get; set; } = new List<Dietplan>();

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<UserLoginToken> UserLoginTokens { get; set; } = new List<UserLoginToken>();

    public virtual ICollection<UserPhoto> UserPhotos { get; set; } = new List<UserPhoto>();
}
