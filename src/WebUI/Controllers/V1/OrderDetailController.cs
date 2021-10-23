using Application.Common.Configuration;
using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Common;
using Application.DataTransferObjects.OrderDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    public class OrderDetailController : BaseController
    {

        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderController> _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public OrderDetailController(IOrderDetailService orderDetailService, IOptionsSnapshot<ApplicationConfiguration> applicationConfiguration, ILogger<OrderController> logger)
        {
            _orderDetailService = orderDetailService;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        [HttpGet]
        public virtual async Task<PaginatedList<ShowOrderDetailDto>> Get(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 0)
        {
            pageSize = pageSize == 0 ? _applicationConfiguration.DefaultPageSize : pageSize;
            return await _orderDetailService.SelectAllOrderDetails(pageNumber, pageSize, cancellationToken);
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
