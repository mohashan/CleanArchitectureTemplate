using Application.DataTransferObjects.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Utilities
{
    public static class PaginationExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }
}