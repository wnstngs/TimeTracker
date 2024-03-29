﻿using Microsoft.AspNetCore.Identity;

namespace TimeTracker.Models;

public class ApplicationUser : IdentityUser<int>
{
    public ApplicationUser()
    {
        ClosedWeeks = new List<ClosedWeek>();
    }

    public ICollection<ClosedWeek> ClosedWeeks { get; set; }
}