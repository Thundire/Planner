﻿using System.Data;

namespace Planner.Application.Models;

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public virtual User User { get; set; } = new();
    public virtual Role Role { get; set; } = new();
}
