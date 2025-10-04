using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class UserPhoto
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string? PhotoPath { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual User? User { get; set; }
}
