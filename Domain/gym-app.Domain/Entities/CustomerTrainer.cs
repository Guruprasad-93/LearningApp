using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class CustomerTrainer
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public long? TrainerId { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Trainer? Trainer { get; set; }

    public virtual User? User { get; set; }
}
