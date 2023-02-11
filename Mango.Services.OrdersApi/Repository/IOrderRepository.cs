using Mango.Services.OrdersApi.Models;

namespace Mango.Services.OrdersApi.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatus (int orderHeaderId,bool paid);
    }
}
