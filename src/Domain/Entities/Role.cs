using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role : AuditableEntity,IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}