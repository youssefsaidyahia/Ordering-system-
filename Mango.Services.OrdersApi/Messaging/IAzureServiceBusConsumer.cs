namespace Mango.Services.OrdersApi.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
