using Application.Common.Controller;
using Application.Common.Interfaces;
using Application.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI.Controllers.V1
{
    [ApiVersion("1")]
    public class UserController : BaseController
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
        public virtual async Task<ActionResult> SignIn([FromForm] SignInDto signIn, CancellationToken cancellationToken)
        {
            var result = await _identityService.SignInAsync(signIn, cancellationToken);
            return new JsonResult(result);
        }

    }
}
