using Application.Common.Configuration;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ServiceLifetimes;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService, IScopedDependency
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public JwtService(SignInManager<ApplicationUser> signInManager, IOptionsSnapshot<ApplicationConfiguration> applicationConfiguration)
        {
            _signInManager = signInManager;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        public async Task<string> GenerateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationConfiguration.JwtConfiguration.SecretKey));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var encryptKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationConfiguration.JwtConfiguration.EncryptKey));
            var encryptingCredentials =
                new EncryptingCredentials(encryptKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await GetClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _applicationConfiguration.JwtConfiguration.Issuer,
                Audience = _applicationConfiguration.JwtConfiguration.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_applicationConfiguration.JwtConfiguration.NotBeforeMinutes),
                Expires = DateTime.Now.AddHours(_applicationConfiguration.JwtConfiguration.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(descriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claimsPrincipal = await _signInManager.ClaimsFactory.CreateAsync(user);
            return claimsPrincipal.Claims;
        }
    }
}