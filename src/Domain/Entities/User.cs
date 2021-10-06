using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class User : AuditableEntity, IEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        public string Name { get; set; }
        public string Family { get; set; }
        public string UserName { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }


        public Role Role { get; set; }
    }
}