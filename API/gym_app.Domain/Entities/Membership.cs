using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Membership
{
    public long MembershipId { get; set; }

    public long? UserId { get; set; }

    public int Duration { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public long? TrainerId { get; set; }

    public long? Type { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual User? User { get; set; }
}
