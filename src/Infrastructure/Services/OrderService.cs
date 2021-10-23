using Application.Common.Api;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.ServiceLifetimes;
using Application.Common.Utilities;
using Application.DataTransferObjects.Common;
using Application.DataTransferObjects.Order;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService, IScopedDependency
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<Order> orderRepository, IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedList<ShowOrderDto>> SelectAllOrders(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var paginatedOrders = await _orderRepository.TableNoTracking.ProjectTo<ShowOrderDto>(_mapper.ConfigurationProvider).PaginatedListAsync(pageNumber, pageSize);

            if (paginatedOrders.Items == null || !paginatedOrders.Items.Any())
            {
                throw new AppException(ApiResultStatusCode.ListEmpty, ApiResultStatusCode.ListEmpty.ToString());
            }

            return paginatedOrders;
        }

        public async Task<ShowOrderDto> SelectOrder(int orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.TableNoTracking.ProjectTo<ShowOrderDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException(ApiResultStatusCode.NotFound.ToDisplay());
            }

            return order;
        }

        public async Task<ShowOrderDto> InsertOrder(CreateOrderDto order, CancellationToken cancellationToken)
        {
            var newOrder = order.ToEntity(_mapper);
            await _orderRepository.AddAsync(newOrder, cancellationToken);

            return ShowOrderDto.FromEntity(_mapper, newOrder);
        }

        public async Task<ShowOrderDto> UpdateOrder(int orderId, UpdateOrderDto order, CancellationToken cancellationToken)
        {
            if (orderId != order.Id)
            {
                throw new BadRequestException("There are no such order available");
            }

            var oldOrder = await _orderRepository.GetByIdAsync(cancellationToken, orderId);

            var newOrder = order.ToEntity(_mapper, oldOrder);
            await _orderRepository.UpdateAsync(newOrder, cancellationToken);

            return ShowOrderDto.FromEntity(_mapper, newOrder);
        }

        public async Task<ShowOrderDto> DeleteOrder(int orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(cancellationToken, orderId);
            if (order == null)
            {
                throw new BadRequestException("There are no such order available");
            }

            await _orderRepository.DeleteAsync(order, cancellationToken);
            return ShowOrderDto.FromEntity(_mapper, order);
        }

        public async Task<ShowOrderWithDetails> SelectOrderWithDetail(int orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.TableNoTracking.Where(o => o.Id == orderId).ProjectTo<ShowOrderWithDetails>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                throw new NotFoundException(ApiResultStatusCode.NotFound.ToDisplay());
            }

            return order;
        }
    }
}