using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAO
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        { 
        
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<ProductItem> ProductItem { get; set; }

        public DbSet<ProductOrderMaster> ProductOrderMaster { get; set; }
        public DbSet<ProductOrderDetail> ProductOrderDetail { get; set; }

    }
}
