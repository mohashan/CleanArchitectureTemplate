using Application.Common.Api;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.ServiceLifetimes;
using Application.Common.Utilities;
using Application.DataTransferObjects.OrderDetail;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderDetailService : IOrderDetailService, IScopedDependency
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(IRepository<OrderDetail> orderDetailRepository, IRepository<Order> orderRepository, IRepository<Product> productRepository, IMapper mapper, ILogger<OrderDetailService> logger)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ShowOrderDetailDto>> SelectAllOrderDetails(CancellationToken cancellationToken)
        {
            var orderDetails = await _orderDetailRepository.TableNoTracking.ProjectTo<ShowOrderDetailDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (orderDetails == null || !orderDetails.Any())
            {
                throw new AppException(ApiResultStatusCode.ListEmpty, ApiResultStatusCode.ListEmpty.ToString());
            }

            return orderDetails;
        }

        public async Task<ShowOrderDetailDto> SelectOrderDetail(int orderDetailId, CancellationToken cancellationToken)
        {
            var orderDetail = await _orderDetailRepository.TableNoTracking.ProjectTo<ShowOrderDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == orderDetailId, cancellationToken);

            if (orderDetail == null)
            {
                throw new NotFoundException(ApiResultStatusCode.NotFound.ToDisplay());
            }

            return orderDetail;
        }

        public async Task<ShowOrderDetailDto> InsertOrderDetail(CreateOrderDetailDto orderDetail, CancellationToken cancellationToken)
        {
            if (!await _orderRepository.TableNoTracking.AnyAsync(o => o.Id == orderDetail.OrderId, cancellationToken))
            {
                throw new BadRequestException("This Order Not Exist");
            }

            if (!await _productRepository.TableNoTracking.AnyAsync(p => p.Id == orderDetail.ProductId,
                cancellationToken))
            {
                throw new BadRequestException("This Product Not Exist");
            }

            var newOrder = orderDetail.ToEntity(_mapper);
            await _orderDetailRepository.AddAsync(newOrder, cancellationToken);

            return ShowOrderDetailDto.FromEntity(_mapper, newOrder);
        }

        public async Task<ShowOrderDetailDto> UpdateOrderDetail(int orderDetailId, UpdateOrderDetailDto orderDetail, CancellationToken cancellationToken)
        {
            if (orderDetailId != orderDetail.Id)
            {
                throw new BadRequestException("There are no such order detail available");
            }

            if (!await _productRepository.TableNoTracking.AnyAsync(p => p.Id == orderDetail.ProductId,
                cancellationToken))
            {
                throw new BadRequestException("This Product Not Exist");
            }

            var oldOrderDetail = await _orderDetailRepository.GetByIdAsync(cancellationToken, orderDetailId);

            var newOrderDetail = orderDetail.ToEntity(_mapper, oldOrderDetail);
            await _orderDetailRepository.UpdateAsync(newOrderDetail, cancellationToken);

            return ShowOrderDetailDto.FromEntity(_mapper, newOrderDetail);
        }

        public async Task<ShowOrderDetailDto> DeleteOrderDetail(int orderDetailId, CancellationToken cancellationToken)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(cancellationToken, orderDetailId);
            if (orderDetail == null)
            {
                throw new BadRequestException("There are no such order detail available");
            }

            await _orderDetailRepository.DeleteAsync(orderDetail, cancellationToken);
            return ShowOrderDetailDto.FromEntity(_mapper, orderDetail);
        }
    }
}