using Microsoft.EntityFrameworkCore;
using ValeShop.Models.Entities;

namespace ValeShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<BillingDetails> BillingDetails { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }   
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPromos> ProductPromos { get; set; }
        public DbSet<Promos> Promos { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
