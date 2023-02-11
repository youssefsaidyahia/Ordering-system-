using Mango.Services.ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> products{ get; set; }
        public DbSet<CartHeader> cartHeaders{ get; set; }
        public DbSet<CartDetails> CartDetails{ get; set; }

    }
}
