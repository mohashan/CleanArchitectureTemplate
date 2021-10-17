using Application.DataTransferObjects.OrderDetail;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IOrderDetailService
    {
        #region OrderDetail

        Task<List<ShowOrderDetailDto>> SelectAllOrderDetails(CancellationToken cancellationToken);
        Task<ShowOrderDetailDto> SelectOrderDetail(int orderDetailId, CancellationToken cancellationToken);
        Task<ShowOrderDetailDto> InsertOrderDetail(CreateOrderDetailDto orderDetail, CancellationToken cancellationToken);
        Task<ShowOrderDetailDto> UpdateOrderDetail(int orderDetailId, UpdateOrderDetailDto orderDetail, CancellationToken cancellationToken);
        Task<ShowOrderDetailDto> DeleteOrderDetail(int orderDetailId, CancellationToken cancellationToken);

        #endregion
    }
}