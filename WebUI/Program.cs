using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using PlaneStore.Application;
using PlaneStore.Application.Models;
using PlaneStore.Infrastructure;
using PlaneStore.Infrastructure.Data;
using PlaneStore.WebUI.Services;
using System.Globalization;

namespace PlaneStore.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<Cart>(SessionCart.GetSessionCart);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
                app.EnsureDatabasePopulated();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePages();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute("page", "Page{currentPage}", new { Controller = "Home", action = "Index" });
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}