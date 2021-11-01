using Application.Common.Configuration;
using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Common;
using Application.DataTransferObjects.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    [ApiVersion("1")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ProductController(IProductService productService, IMapper mapper, IOptionsSnapshot<ApplicationConfiguration> applicationConfiguration)
        {
            _productService = productService;
            _mapper = mapper;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        [HttpGet]
        public virtual async Task<PaginatedList<ShowProductDto>> Get(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 0)
        {
            pageSize = pageSize == 0 ? _applicationConfiguration.DefaultPageSize : pageSize;
            return await _productService.SelectAllProducts(pageNumber, pageSize, cancellationToken);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<ShowProductDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _productService.SelectProduct(id, cancellationToken);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShowProductDto>> Create([FromBody] CreateProductDto product, CancellationToken cancellationToken)
        {
            return await _productService.InsertProduct(product, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<ShowProductDto>> Update([FromRoute] int id, [FromBody] UpdateProductDto product,
            CancellationToken cancellationToken)
        {
            return await _productService.UpdateProduct(id, product, cancellationToken);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<ShowProductDto>> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _productService.DeleteProduct(id, cancellationToken);
        }
    }
}
