using Application.DataTransferObjects.Common;

namespace Application.DataTransferObjects.OrderDetail
{
    public class ShowOrderDetailDto : BaseDto<ShowOrderDetailDto, Domain.Entities.OrderDetail>
    {
        public string OrderUserName { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public int Amount { get; set; }
    }
}