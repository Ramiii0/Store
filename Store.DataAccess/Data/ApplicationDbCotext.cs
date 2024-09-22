using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Models.Models;

namespace MyStore.Data
{
    public class ApplicationDbCotext: IdentityDbContext<AppUser>
    {
        public ApplicationDbCotext(DbContextOptions<ApplicationDbCotext> options) : base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
