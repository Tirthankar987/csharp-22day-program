using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class Diagnosis
{
    public int DiagnosisId { get; set; }

    public int EncounterId { get; set; }

    public string IcdCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DiagnosedOn { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;
}
