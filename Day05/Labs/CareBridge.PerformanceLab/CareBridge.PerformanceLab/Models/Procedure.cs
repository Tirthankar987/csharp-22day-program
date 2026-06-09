using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class Procedure
{
    public int ProcedureId { get; set; }

    public int EncounterId { get; set; }

    public string CptCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime PerformedOn { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;
}
