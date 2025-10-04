using System;
using System.Collections.Generic;

namespace gym_app.Domain.Entities;

public partial class Customer
{
    public int Accno { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public int? Account { get; set; }

    public int? Status { get; set; }
}
