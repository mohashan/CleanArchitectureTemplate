using Application.DataTransferObjects.Common;

namespace Application.DataTransferObjects.Order
{
    public class CreateOrderDto : BaseDto<CreateOrderDto, Domain.Entities.Order>
    {
        public string UserName { get; set; }
        public int Amount { get; set; }
    }
}