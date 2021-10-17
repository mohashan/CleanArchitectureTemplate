using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public virtual async Task<List<ShowOrderDto>> Get(CancellationToken cancellationToken)
        {
            return await _orderService.SelectAllOrders(cancellationToken);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<ShowOrderDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _orderService.SelectOrder(id, cancellationToken);
        }

        [HttpGet("OrderWithDetail/{id}")]
        public virtual async Task<ActionResult<ShowOrderWithDetails>> OrderWithDetail([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _orderService.SelectOrderWithDetail(id, cancellationToken);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShowOrderDto>> Create([FromBody] CreateOrderDto product, CancellationToken cancellationToken)
        {
            return await _orderService.InsertOrder(product, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<ShowOrderDto>> Update([FromRoute] int id, [FromBody] UpdateOrderDto product,
            CancellationToken cancellationToken)
        {
            return await _orderService.UpdateOrder(id, product, cancellationToken);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<ShowOrderDto>> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _orderService.DeleteOrder(id, cancellationToken);
        }
    }
}
