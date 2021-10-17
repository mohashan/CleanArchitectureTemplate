using Application.DataTransferObjects.Common;

namespace Application.DataTransferObjects.Order
{
    public class UpdateOrderDto : BaseDto<UpdateOrderDto, Domain.Entities.Order>
    {
        public string UserName { get; set; }
        public int Amount { get; set; }
    }
}