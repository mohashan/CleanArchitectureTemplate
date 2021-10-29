using System;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order : IAuditableEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Amount { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        


        public ICollection<OrderDetail> OrderDetails { get; set; }
        
    }
}