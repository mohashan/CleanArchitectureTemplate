using Application.Common.Configuration;
using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Common;
using Application.DataTransferObjects.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    [ApiVersion("1")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public OrderController(IOrderService orderService, IOptionsSnapshot<ApplicationConfiguration> applicationConfiguration, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        [HttpGet]
        public virtual async Task<PaginatedList<ShowOrderDto>> Get(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 0)
        {
            pageSize = pageSize == 0 ? _applicationConfiguration.DefaultPageSize : pageSize;
            return await _orderService.SelectAllOrders(pageNumber,pageSize,cancellationToken);
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
