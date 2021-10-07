using Application.Common.Filters;
using Application.Common.Interfaces;
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
        public async Task<List<Product>> Get(CancellationToken cancellationToken)
        {
            var products = await _productRepository.TableNoTracking.ToListAsync(cancellationToken);
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product =
                await _productRepository.TableNoTracking.Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productRepository.AddAsync(product, cancellationToken);

            return product;
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Update([FromRoute] int id, [FromBody] Product product,
            CancellationToken cancellationToken)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productRepository.UpdateAsync(product, cancellationToken);

            return product;
        }

        [HttpDelete]
        public async Task<ActionResult<Product>> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(cancellationToken, id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(product, cancellationToken);
            return product;
        }
    }
}
