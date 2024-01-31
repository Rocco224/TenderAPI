﻿using System;
using System.Collections.Generic;

namespace TenderAPI.Models;

public partial class User
{
    public int? UserId { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public string? Salt { get; set; }

    public string? Username { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }
}