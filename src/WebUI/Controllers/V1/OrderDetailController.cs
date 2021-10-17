using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.OrderDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    public class OrderDetailController : BaseController
    {

        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderController> _logger;

        public OrderDetailController(IOrderDetailService orderDetailService, ILogger<OrderController> logger)
        {
            _orderDetailService = orderDetailService;
            _logger = logger;
        }

        [HttpGet]
        public virtual async Task<List<ShowOrderDetailDto>> Get(CancellationToken cancellationToken)
        {
            return await _orderDetailService.SelectAllOrderDetails(cancellationToken);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<ShowOrderDetailDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _orderDetailService.SelectOrderDetail(id, cancellationToken);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShowOrderDetailDto>> Create([FromBody] CreateOrderDetailDto orderDetail, CancellationToken cancellationToken)
        {
            return await _orderDetailService.InsertOrderDetail(orderDetail, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<ShowOrderDetailDto>> Update([FromRoute] int id, [FromBody] UpdateOrderDetailDto orderDetail,
            CancellationToken cancellationToken)
        {
            return await _orderDetailService.UpdateOrderDetail(id, orderDetail, cancellationToken);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<ShowOrderDetailDto>> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _orderDetailService.DeleteOrderDetail(id, cancellationToken);
        }

    }
}
