using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class Encounter
{
    public int EncounterId { get; set; }

    public int PatientId { get; set; }

    public int? ProviderId { get; set; }

    public int DepartmentId { get; set; }

    public DateTime AdmitDate { get; set; }

    public DateTime? DischargeDate { get; set; }

    public string EncounterType { get; set; } = null!;

    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();

    public virtual Provider? Provider { get; set; }
}
