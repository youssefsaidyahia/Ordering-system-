using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.OrdersPaymentsApi.Messages;
using Newtonsoft.Json;
using PaymentProcessor;
using System.Text;

namespace Mango.Services.OrdersPaymentsApi.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly string serviceBusConnectionString;
        private readonly string subscriptionPayment;
        private readonly string OrderPaymentProcessTopic;
        private readonly string OrderUpdatePaymentResultTopic;

        private ServiceBusProcessor OrderPaymentProcesser;
        private readonly IMessageBus _messageBus;
        private readonly IProcessPayment _processPayment;
        private readonly IConfiguration _configuration;
        public AzureServiceBusConsumer(IProcessPayment processPayment, IConfiguration configuration, IMessageBus messageBus)
        {
            _processPayment = processPayment;
            _configuration = configuration;
            _messageBus = messageBus;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionPayment = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
            OrderPaymentProcessTopic = _configuration.GetValue<string>("orderpaymentprocesstopic");
            OrderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");


            var client = new ServiceBusClient(serviceBusConnectionString);

            OrderPaymentProcesser = client.CreateProcessor(OrderPaymentProcessTopic, subscriptionPayment);
        }
        public async Task Start()
        {
            OrderPaymentProcesser.ProcessMessageAsync += ProcessPayments;
            OrderPaymentProcesser.ProcessErrorAsync += ErrorHandler;
            await OrderPaymentProcesser.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await OrderPaymentProcesser.StopProcessingAsync();
            await OrderPaymentProcesser.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessPayments(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage payment = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);
            var result = _processPayment.paymentProcessor();
            UpdatePaymentResultMessage resultMessage = new()
            {
                status = true,
                OrderId = payment.OrderId,
                Email = payment.Email
            };

            try
            {
                await _messageBus.PublishMessage(resultMessage, OrderUpdatePaymentResultTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ) 
            {
                throw;
            }
        }
    }
}
