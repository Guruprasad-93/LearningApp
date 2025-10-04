using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Dietplan
{
    public long DietplanId { get; set; }

    public long? UserId { get; set; }

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual User? User { get; set; }
}
