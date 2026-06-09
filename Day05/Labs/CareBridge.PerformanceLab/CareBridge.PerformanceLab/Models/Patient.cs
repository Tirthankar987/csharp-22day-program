using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string Mrn { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? City { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
}
