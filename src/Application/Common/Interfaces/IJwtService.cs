using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Application.DataTransferObjects.User;

namespace Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<SignInResultDto> GenerateAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}