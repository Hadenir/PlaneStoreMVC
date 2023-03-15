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

            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddAutoMapper();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<Cart>(SessionCart.GetSessionCart);

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/identity/account/login";
                options.LogoutPath = "/identity/account/logout";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
                app.EnsureDatabasePopulated();
                app.UseStatusCodePages();
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

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                "admin",
                "Admin/{controller=Aircraft}/{action=Index}/{id?}",
                defaults: new { area = "Admin" },
                constraints: new { area = "Admin" });
            app.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("page", "Page{currentPage}", new { controller = "Home", action = "Index" });
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}