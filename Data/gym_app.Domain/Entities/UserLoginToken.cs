using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class UserLoginToken
{
    public long TokenId { get; set; }

    public long? UserId { get; set; }

    public string? UserName { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? Expires { get; set; }

    public bool IsExpired { get; set; }

    public bool IsActive { get; set; }

    public string? JwtToken { get; set; }

    public long? RevokedBy { get; set; }

    public DateTime? Revoked { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public string? ReplacedByToken { get; set; }

    public string? SessionId { get; set; }

    public string? CrfToken { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual User? User { get; set; }
}
