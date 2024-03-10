using System;
using System.Collections.Generic;

namespace TenderAPI.Models;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public int PracticeId { get; set; }

    public string Path { get; set; } = null!;

    public virtual Practice Practice { get; set; } = null!;
}
