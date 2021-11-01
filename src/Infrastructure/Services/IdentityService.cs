using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.ServiceLifetimes;
using Application.DataTransferObjects.User;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService, IScopedDependency
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<SignInResultDto> SignInAsync(SignInDto signin, CancellationToken cancellationToken)
        {
            if (signin.grant_type.Equals(nameof(signin.password), StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException("OAuth flow is not password");
            }

            var user = await _userManager.FindByNameAsync(signin.username);
            if (user == null)
            {
                throw new BadRequestException("There is no user with this username");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, signin.password);
            if (!isPasswordValid)
            {
                throw new BadRequestException("Password is not valid");
            }

            return await _jwtService.GenerateAsync(user, cancellationToken);
        }
    }
}