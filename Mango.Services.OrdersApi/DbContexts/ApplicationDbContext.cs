
using Mango.Services.OrdersApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrdersApi.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<OrderHeader>  OrderHeaders{ get; set; }
        public DbSet<OrderDetails>  OrderDetails{ get; set; }

    }
}
