using Mango.MessageBus;

namespace Mango.Services.OrdersApi.RabbitMQSender
{
    public interface IRabbitMQOrderSender
    {
        void SendMessage(BaseMessage baseMessage, String queueName);
    }
}
