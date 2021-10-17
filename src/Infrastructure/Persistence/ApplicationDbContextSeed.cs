using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var administratorRole = new ApplicationRole
            {
                Name = "Administrator",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = "0",
                IsDeleted = false
            };

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                try
                {
                    await roleManager.CreateAsync(administratorRole);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            var administrator = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                BirthDay = new DateTime(1994, 04, 25, 6, 30, 0),
                Gender = Gender.Man,
                Name = "Reza",
                Family = "Neyestani",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = "0",
                IsDeleted = false
            };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "P@ssw0rd1234!!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }
    }
}