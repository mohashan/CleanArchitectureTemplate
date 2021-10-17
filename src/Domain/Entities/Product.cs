﻿using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}