﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlaneStore.Domain.DataAccess;
using PlaneStore.Infrastructure.Data;
using PlaneStore.Infrastructure.DataAccess;

namespace PlaneStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection")
                ?? throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static IApplicationBuilder EnsureDatabasePopulated(this IApplicationBuilder app)
        {
            SeedData.EnsurePopulated(app);

            return app;
        }
    }
}
