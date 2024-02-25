using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TenderAPI.Models;

public partial class ProceduresType
{
    public int ProcedureTypeId { get; set; }

    public string Description { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}
