using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Data
{
    public class ApplicationDbCotext: IdentityDbContext<AppUser>
    {
        public ApplicationDbCotext(DbContextOptions<ApplicationDbCotext> options) : base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
