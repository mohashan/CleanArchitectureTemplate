using Application.DataTransferObjects.Common;

namespace Application.DataTransferObjects.Order
{
    public class ShowOrderDto : BaseDto<ShowOrderDto, Domain.Entities.Order>
    {
        public string UserName { get; set; }
        public int Amount { get; set; }
    }
}