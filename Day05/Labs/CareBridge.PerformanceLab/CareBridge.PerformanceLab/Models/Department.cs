using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
