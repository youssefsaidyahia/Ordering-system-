using Mango.MessageBus;

namespace Mango.Services.OrdersPaymentsApi.RabbitMQSender
{
    public interface IRabbitMQPaymentMessageSender
    {
        void SendMessage(BaseMessage baseMessage);
    }
}
