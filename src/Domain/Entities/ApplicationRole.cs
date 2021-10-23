using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class ApplicationRole : IdentityRole<int>, IAuditableEntity
    {
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}