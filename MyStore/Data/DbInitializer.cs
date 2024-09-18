using Microsoft.AspNetCore.Identity;
using MyStore.Models;

namespace MyStore.Data
{
    public class DbInitializer
    {

        public static async Task SeedData(UserManager<AppUser>? userManager,RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                return;
            }
            //checkif we have the admin role
            if(! await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (!await roleManager.RoleExistsAsync("seller"))
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            if (!await roleManager.RoleExistsAsync("client"))
            {
                await roleManager.CreateAsync(new IdentityRole("client"));
            }
            var adminuser = await userManager.GetUsersInRoleAsync("admin");
            if (adminuser.Any())
            {
                return;
            }
            var newuser = new AppUser()
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                CreatedAt = DateTime.Now,
            };
            string pass = "admin@1234";
            var result= await userManager.CreateAsync(newuser, pass);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newuser,"admin");
                return;
            }
        }
    }
}
