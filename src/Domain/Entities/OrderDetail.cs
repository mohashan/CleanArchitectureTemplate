using Domain.Common;
using System;

namespace Domain.Entities
{
    public class OrderDetail : IAuditableEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Amount { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}