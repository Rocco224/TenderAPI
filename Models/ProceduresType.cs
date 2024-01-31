using System;
using System.Collections.Generic;

namespace TenderAPI.Models;

public partial class ProceduresType
{
    public int ProcedureTypeId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}
