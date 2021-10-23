using Application.DataTransferObjects.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DataTransferObjects.Common;

namespace Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedList<ShowProductDto>> SelectAllProducts(int pageNumber,int pageSize,CancellationToken cancellationToken);
        Task<ShowProductDto> SelectProduct(int productId, CancellationToken cancellationToken);
        Task<ShowProductDto> InsertProduct(CreateProductDto product, CancellationToken cancellationToken);
        Task<ShowProductDto> UpdateProduct(int productId, UpdateProductDto product, CancellationToken cancellationToken);
        Task<ShowProductDto> DeleteProduct(int productId, CancellationToken cancellationToken);
    }
}