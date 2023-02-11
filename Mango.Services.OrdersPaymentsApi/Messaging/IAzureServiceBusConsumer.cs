namespace Mango.Services.OrdersPaymentsApi.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
