using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DataTransferObjects.User;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
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
            var user = await _userManager.FindByNameAsync(signin.UserName);
            if (user == null)
            {
                throw new BadRequestException("There is no user with this username");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, signin.Password);
            if (!isPasswordValid)
            {
                throw new BadRequestException("Password is not valid");
            }

            var token = await _jwtService.GenerateAsync(user, cancellationToken);

            return new SignInResultDto
            {
                UserName = signin.UserName,
                Token = token
            };
        }
    }
}