using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyStore.Data;
using MyStore.Models;

namespace MyStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbCotext>(options =>

            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                
            }
            ).AddEntityFrameworkStores<ApplicationDbCotext>().AddDefaultTokenProviders();
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            using (var scope = app.Services.CreateScope())
            {
                var usermanger= scope.ServiceProvider.GetService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;
                var rolemanger = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;
                await DbInitializer.SeedData(usermanger, rolemanger);

            }

            app.Run();
        }
    }
}
