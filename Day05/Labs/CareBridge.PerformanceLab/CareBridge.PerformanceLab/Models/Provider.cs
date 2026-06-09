using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string FullName { get; set; } = null!;

    public string Specialty { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}
