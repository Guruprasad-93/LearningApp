using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class GymType
{
    public long TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }
}
