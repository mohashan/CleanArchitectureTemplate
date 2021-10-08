using Application.DataTransferObjects.User;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<SignInResultDto> SignInAsync(SignInDto signin, CancellationToken cancellationToken);
    }
}