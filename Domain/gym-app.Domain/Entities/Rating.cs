using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Rating
{
    public long RatingId { get; set; }

    public long? UserId { get; set; }

    public int? Stars { get; set; }

    public string? Review { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual User? User { get; set; }
}
