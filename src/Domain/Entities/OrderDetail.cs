using Domain.Common;

namespace Domain.Entities
{
    public class OrderDetail:IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Amount { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}