using Mango.MessageBus;

namespace Mango.Services.OrdersPaymentsApi.Messages
{
    public class UpdatePaymentResultMessage :BaseMessage
    {
        public int OrderId { get; set; }
        public bool status { get; set; }
        public string Email { get; set; }
    }
}
