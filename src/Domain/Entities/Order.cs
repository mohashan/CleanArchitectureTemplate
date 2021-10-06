using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order : AuditableEntity, IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Amount { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}