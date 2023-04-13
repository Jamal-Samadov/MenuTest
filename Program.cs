using BookmallMenu.Data;
using CodeofAzerbaijan.DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookmallMenu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc();

            builder.Services
                .AddSession(opt => opt.IdleTimeout = TimeSpan.FromSeconds(45));


            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            //builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer());

            builder.Services.AddDbContext<AppDbContext>(
               opt => opt.UseSqlServer(builder.Configuration
               .GetConnectionString("DefaultConnection"),
               builder =>
               {
                   builder.MigrationsAssembly("BookmallMenu");
               }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.AppPath = Path.Combine(Constants.RootPath, "assets", "img");
            Constants.FoodPath = Path.Combine(Constants.RootPath, "assets", "img", "food");


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.Run();
        }
    }
}