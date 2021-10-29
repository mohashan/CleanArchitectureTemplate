using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public static async Task SeedDefaultProductAndOrderAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (!context.Set<Product>().Any())
            {
                await SeedProductAsync(context, cancellationToken);
                await SeedOrderAsync(context, cancellationToken);
            }
        }

        private static async Task SeedProductAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var random = new Random();
            for (var i = 1; i < 21; i++)
            {
                await context.Set<Product>().AddAsync(new Product
                {
                    Name = $"Product {i}",
                    Description = $"Description For Product {i}",
                    Amount = random.Next(1, 1000) * 1000,
                    IsDeleted = false
                }, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task SeedOrderAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var orderDetailList = new List<OrderDetail>();
            var productList = await context.Set<Product>().OrderBy(p => p.Id).ToListAsync(cancellationToken);
            var random = new Random();

            for (var i = 1; i < 16; i++)
            {
                var maxOrderCount = random.Next(2, 11);
                for (var j = 1; j < maxOrderCount; j++)
                {
                    var productId = random.Next(productList.Min(p => p.Id), productList.Max(p => p.Id));

                    if (orderDetailList.All(p => p.Id != productId))
                    {
                        orderDetailList.Add(new OrderDetail
                        {
                            ProductId = productId,
                            Amount = productList.Where(p => p.Id == productId).Select(p => p.Amount).First(),
                            Count = random.Next(1, 10),
                            IsDeleted = false
                        });
                    }
                }

                var order = new Order
                {
                    UserName = "Seeder",
                    Amount = orderDetailList.Sum(orderDetail => orderDetail.Count * orderDetail.Amount),
                    IsDeleted = false,
                };

                await context.Set<Order>().AddAsync(order, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                orderDetailList.ForEach(o => o.OrderId = order.Id);

                await context.Set<OrderDetail>().AddRangeAsync(orderDetailList, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                orderDetailList.Clear();


            }
        }
    }
}