using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<List<ShowProductDto>> Get(CancellationToken cancellationToken)
        {
            return await _productService.SelectAllProducts(cancellationToken);
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
