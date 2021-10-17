using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}