using Application.Common.Api;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.ServiceLifetimes;
using Application.Common.Utilities;
using Application.DataTransferObjects.Common;
using Application.DataTransferObjects.Products;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : IProductService, IScopedDependency
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepository<Product> productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedList<ShowProductDto>> SelectAllProducts(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var paginatedProducts = await _productRepository
                .TableNoTracking
                .ProjectTo<ShowProductDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(pageNumber, pageSize);

            if (paginatedProducts.Items == null || !paginatedProducts.Items.Any())
            {
                throw new AppException(ApiResultStatusCode.ListEmpty, ApiResultStatusCode.ListEmpty.ToDisplay());
            }

            return paginatedProducts;
        }

        public async Task<ShowProductDto> SelectProduct(int productId, CancellationToken cancellationToken)
        {
            var product =
                await _productRepository.TableNoTracking.Where(p => p.Id == productId)
                    .ProjectTo<ShowProductDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(ApiResultStatusCode.NotFound.ToDisplay());
            }

            return product;
        }

        public async Task<ShowProductDto> InsertProduct(CreateProductDto product, CancellationToken cancellationToken)
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

        public async Task<ShowProductDto> UpdateProduct(int productId, UpdateProductDto product, CancellationToken cancellationToken)
        {
            if (productId != product.Id)
            {
                throw new BadRequestException("There are no such products available");
            }

            if (await _productRepository.TableNoTracking
                .AnyAsync(p => p.Name == product.Name && p.Id != productId, cancellationToken))
            {
                throw new BadRequestException("Product with this name is exist");
            }

            var oldProduct = await _productRepository.GetByIdAsync(cancellationToken, productId);

            var newProduct = product.ToEntity(_mapper, oldProduct);
            await _productRepository.UpdateAsync(newProduct, cancellationToken);

            return ShowProductDto.FromEntity(_mapper, newProduct);
        }

        public async Task<ShowProductDto> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(cancellationToken, productId);
            if (product == null)
            {
                throw new BadRequestException("There are no such products available");
            }

            await _productRepository.DeleteAsync(product, cancellationToken);
            return ShowProductDto.FromEntity(_mapper, product);
        }
    }
}