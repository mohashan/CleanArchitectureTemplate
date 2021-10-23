using Application.DataTransferObjects.Order;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DataTransferObjects.Common;

namespace Application.Common.Interfaces
{
    public interface IOrderService
    {
        #region Order

        Task<PaginatedList<ShowOrderDto>> SelectAllOrders(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ShowOrderDto> SelectOrder(int orderId, CancellationToken cancellationToken);
        Task<ShowOrderDto> InsertOrder(CreateOrderDto order, CancellationToken cancellationToken);
        Task<ShowOrderDto> UpdateOrder(int orderId, UpdateOrderDto order, CancellationToken cancellationToken);
        Task<ShowOrderDto> DeleteOrder(int orderId, CancellationToken cancellationToken);

        #endregion

        #region OrderDetail

        Task<ShowOrderWithDetails> SelectOrderWithDetail(int orderId, CancellationToken cancellationToken);

        #endregion
    }
}