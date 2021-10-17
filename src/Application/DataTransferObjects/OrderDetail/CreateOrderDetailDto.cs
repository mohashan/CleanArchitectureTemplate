using Application.DataTransferObjects.Common;

namespace Application.DataTransferObjects.OrderDetail
{
    public class CreateOrderDetailDto : BaseDto<CreateOrderDetailDto, Domain.Entities.OrderDetail>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Amount { get; set; }
    }
}