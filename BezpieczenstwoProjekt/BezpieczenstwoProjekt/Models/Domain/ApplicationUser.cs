﻿using Microsoft.AspNetCore.Identity;

namespace BezpieczenstwoProjekt.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
