using Application.Common.Exceptions;
using Application.Common.Filters;
using Application.Common.Interfaces;
using Application.DataTransferObjects.Products;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<List<ShowProductDto>> Get(CancellationToken cancellationToken)
        {
            var products = await _productRepository.TableNoTracking.Select(p => new ShowProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Amount = p.Amount,
                Description = p.Description
            }).ToListAsync(cancellationToken);
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShowProductDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product =
                await _productRepository.TableNoTracking.Where(p => p.Id == id).Select(p => new ShowProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Amount = p.Amount,
                    Description = p.Description
                }).FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<ShowProductDto>> Create([FromBody] CreateProductDto product, CancellationToken cancellationToken)
        {
            var oldProduct = await _productRepository.TableNoTracking.FirstOrDefaultAsync(p => p.Name == product.Name, cancellationToken);

            if (oldProduct != null)
            {
                throw new BadRequestException("Product with this name is exist");
            }

            var newProduct = new Product
            {
                Name = product.Name,
                Amount = product.Amount,
                Description = product.Description
            };

            await _productRepository.AddAsync(newProduct, cancellationToken);

            return new ShowProductDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Amount = newProduct.Amount,
                Description = newProduct.Description
            };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShowProductDto>> Update([FromRoute] int id, [FromBody] UpdateProductDto product,
            CancellationToken cancellationToken)
        {
            if (id != product.Id)
            {
                throw new BadRequestException("There are no such products available");
            }

            var oldProduct = await _productRepository.TableNoTracking.FirstOrDefaultAsync(p => p.Name == product.Name && p.Id != id, cancellationToken);

            if (oldProduct != null)
            {
                throw new BadRequestException("Product with this name is exist");
            }

            var newProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Amount = product.Amount,
                Description = product.Description
            };

            await _productRepository.UpdateAsync(newProduct, cancellationToken);

            return new ShowProductDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Amount = newProduct.Amount,
                Description = newProduct.Description
            };
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ShowProductDto>> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(cancellationToken, id);
            if (product == null)
            {
                throw new BadRequestException("There are no such products available");
            }

            await _productRepository.DeleteAsync(product, cancellationToken);
            return new ShowProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Amount = product.Amount,
                Description = product.Description
            };
        }
    }
}
