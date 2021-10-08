using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}