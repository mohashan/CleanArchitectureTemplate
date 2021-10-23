using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}