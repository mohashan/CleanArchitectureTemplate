using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order : AuditableEntity
    {
        public string UserName { get; set; }
        public int Amount { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}