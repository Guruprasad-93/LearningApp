using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Trainer
{
    public long TrainerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Speciality { get; set; }

    public string? Contact { get; set; }

    public bool IsActive { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public long CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CustomerTrainer> CustomerTrainers { get; set; } = new List<CustomerTrainer>();
}
