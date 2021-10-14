using Application.Common.Exceptions;
using Application.Common.Filters;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Products;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiResultFilter]
    [Authorize]
    [ApiVersion("1")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<List<ShowProductDto>> Get(CancellationToken cancellationToken)
        {
            var products = await _productRepository
                .TableNoTracking
                .ProjectTo<ShowProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return products;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<ShowProductDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product =
                await _productRepository.TableNoTracking.Where(p => p.Id == id)
                    .ProjectTo<ShowProductDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShowProductDto>> Create([FromBody] CreateProductDto product, CancellationToken cancellationToken)
        {
            if (await _productRepository.TableNoTracking
                .AnyAsync(p => p.Name == product.Name, cancellationToken))
            {
                throw new BadRequestException("Product with this name is exist");
            }

            var newProduct = product.ToEntity(_mapper);
            await _productRepository.AddAsync(newProduct, cancellationToken);

            return ShowProductDto.FromEntity(_mapper, newProduct);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<ShowProductDto>> Update([FromRoute] int id, [FromBody] UpdateProductDto product,
            CancellationToken cancellationToken)
        {
            if (id != product.Id)
            {
                throw new BadRequestException("There are no such products available");
            }

            if (await _productRepository.TableNoTracking
                .AnyAsync(p => p.Name == product.Name && p.Id != id, cancellationToken))
            {
                throw new BadRequestException("Product with this name is exist");
            }

            var newProduct = product.ToEntity(_mapper);
            await _productRepository.UpdateAsync(newProduct, cancellationToken);

            return ShowProductDto.FromEntity(_mapper, newProduct);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<ShowProductDto>> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(cancellationToken, id);
            if (product == null)
            {
                throw new BadRequestException("There are no such products available");
            }

            await _productRepository.DeleteAsync(product, cancellationToken);
            return ShowProductDto.FromEntity(_mapper, product);
        }
    }
}
