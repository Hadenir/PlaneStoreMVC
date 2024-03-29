﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaneStore.Domain.Entities;

namespace PlaneStore.Infrastructure.Data
{
    internal static class SeedData
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            if (!dbContext.Aircraft.Any() && !dbContext.Manufacturers.Any())
            {
                var manufacturers = new[]
                {
                    new Manufacturer { Name = "Airbus" },
                    new Manufacturer { Name = "Boeing" },
                    new Manufacturer { Name = "Antonov" },
                    new Manufacturer { Name = "Textron Aviation" }
                };
                dbContext.Manufacturers.AddRange(manufacturers);

                var aircraft = new[]
                {
                    new Aircraft
                    {
                        Name = "Airbus A320neo",
                        Description = "The most fuel efficient airframe produced by Airbus.",
                        Price = 110_600_000,
                        ManufacturerId = manufacturers[0].Id,
                    },
                    new Aircraft
                    {
                        Name = "Boeing 737-900",
                        Description = "The longest variant of Boeing's 737NG family.",
                        Price = 94_600_000,
                        ManufacturerId = manufacturers[1].Id,
                    },
                    new Aircraft
                    {
                        Name = "Antonov An-225",
                        Description = "The biggest aircraft in the world.",
                        Price = 500_000_000,
                        ManufacturerId = manufacturers[2].Id,
                    },
                    new Aircraft
                    {
                        Name = "Cessna 152",
                        Description = "The Cessna 152 has been out of production for almost forty years, but many are still airworthy and are in regular use for flight training.",
                        Price = 55_000,
                        ManufacturerId = manufacturers[3].Id,
                    },
                    new Aircraft
                    {
                        Name = "Airbus A380-800",
                        Description = "The biggest passenger Airbus aircraft.",
                        Price = 445_600_000,
                        ManufacturerId = manufacturers[0].Id,
                    },
                    new Aircraft
                    {
                        Name = "Boeing 747-8",
                        Description = "The biggest passenger Boeing aircraft.",
                        Price = 418_400_000,
                        ManufacturerId = manufacturers[1].Id,
                    },
                    new Aircraft
                    {
                        Name = "Boeing 777-9",
                        Description = "The world's larget twin-jet.",
                        Price = 442_200_000,
                        ManufacturerId = manufacturers[1].Id,
                    }
                };
                dbContext.Aircraft.AddRange(aircraft);

                dbContext.SaveChanges();
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var adminUserName = "Admin";
            var adminEmail = "admin@example.com";
            var adminPassword = "Secret123$";
            var user = await userManager.FindByNameAsync(adminUserName);
            if (user is null)
            {
                user = new IdentityUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}
