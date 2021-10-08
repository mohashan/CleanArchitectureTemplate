using Application.Common.Filters;
using Application.Common.Interfaces;
using Application.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<UserController> _logger;

        public UserController(IIdentityService identityService, ILogger<UserController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<SignInResultDto> SignIn([FromBody] SignInDto signIn, CancellationToken cancellationToken)
        {
            var result = await _identityService.SignInAsync(signIn, cancellationToken);
            return result;
        }
    }
}
