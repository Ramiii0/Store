﻿using Microsoft.AspNetCore.Identity;

namespace Store.Models.Models
{
    public class AppUser :IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string?  FirstName  { get; set; }
        public string? LastName { get; set; }
        public string? Address {  get; set; }
            


    }
}
