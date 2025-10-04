using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Company
{
    public long CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompOwnerName { get; set; }

    public string? Address { get; set; }

    public string? PhotoPath { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public long? Createdby { get; set; }

    public long? Modifiedby { get; set; }

    public virtual ICollection<CustomerTrainer> CustomerTrainers { get; set; } = new List<CustomerTrainer>();

    public virtual ICollection<Dietplan> Dietplans { get; set; } = new List<Dietplan>();

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();

    public virtual ICollection<UserLoginToken> UserLoginTokens { get; set; } = new List<UserLoginToken>();

    public virtual ICollection<UserPhoto> UserPhotos { get; set; } = new List<UserPhoto>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
