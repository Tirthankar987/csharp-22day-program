using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class VwClinical
{
    public string Mrn { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int EncounterId { get; set; }

    public string EncounterType { get; set; } = null!;

    public string? IcdCode { get; set; }

    public string? Description { get; set; }
}
