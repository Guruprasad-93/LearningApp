using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Role
{
    public long RoleId { get; set; }

    public string RoleCode { get; set; } = null!;

    public string? RoleName { get; set; }

    public int? RoleLevel { get; set; }

    public bool IsDeleted { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
