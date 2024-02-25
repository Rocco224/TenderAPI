using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TenderAPI.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Description { get; set; } = null!;

    //[JsonIgnore]
    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}
