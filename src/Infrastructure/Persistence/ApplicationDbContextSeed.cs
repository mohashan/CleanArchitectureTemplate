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
                ConcurrencyStamp = Guid.NewGuid().ToString()
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
                BirthDay = DateTime.Now,
                Gender = Gender.Man,
                Name = "Reza",
                Family = "Neyestani",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "P@ssw0rd1234!!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }
    }
}