using Application.DataTransferObjects.Common;

namespace Application.DataTransferObjects.OrderDetail
{
    public class UpdateOrderDetailDto : BaseDto<UpdateOrderDetailDto, Domain.Entities.OrderDetail>
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Amount { get; set; }
    }
}