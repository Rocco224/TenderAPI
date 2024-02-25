using System;
using System.Collections.Generic;

namespace TenderAPI.Models;

public partial class ExpiringPractice
{
    public int PracticeId { get; set; }

    public int CustomerId { get; set; }

    public int ProcedureTypeId { get; set; }

    public int StateId { get; set; }

    public string Authority { get; set; } = null!;

    public string Object { get; set; } = null!;

    public DateTime DateExpire { get; set; }

    public DateTime DateStart { get; set; }

    public string Platform { get; set; } = null!;

    public string ProcurementCode { get; set; } = null!;

    public string PrevalentCategory { get; set; } = null!;

    public double Amount { get; set; }

    public string Note { get; set; } = null!;

    public string Criteria { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string State { get; set; } = null!;
}
