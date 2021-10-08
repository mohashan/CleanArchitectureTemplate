using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
    }
}