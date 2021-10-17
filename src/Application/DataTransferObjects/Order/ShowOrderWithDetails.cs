using Application.DataTransferObjects.Common;
using Application.DataTransferObjects.OrderDetail;
using System.Collections.Generic;

namespace Application.DataTransferObjects.Order
{
    public class ShowOrderWithDetails : BaseDto<ShowOrderWithDetails, Domain.Entities.Order>
    {
        public string UserName { get; set; }
        public int Amount { get; set; }

        public List<ShowOrderDetailDto> OrderDetails { get; set; }
    }
}