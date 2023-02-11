using Mango.Services.OrdersApi.DbContexts;
using Mango.Services.OrdersApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrdersApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
        private readonly string subscriptionName;
        private readonly string subscriptionCheckOut;
        private readonly string checkoutMessageTopic;

        private readonly IConfiguration _configuration;
        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();   
            }   
        }
    }
}
