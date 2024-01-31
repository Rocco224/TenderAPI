using System;
using System.Collections.Generic;

namespace TenderAPI.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}
