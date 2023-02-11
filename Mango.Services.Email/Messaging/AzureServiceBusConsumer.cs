using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly EmailRepository  _emailRepository;

        private readonly string serviceBusConnectionString;
        private readonly string subscriptionEmailLogs;
        private readonly string OrderUpdatePaymentResultTopic;

        private ServiceBusProcessor EmailLogsProcessor;
        private readonly IConfiguration _configuration;
        public AzureServiceBusConsumer(EmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionEmailLogs = _configuration.GetValue<string>("SubscriptionName");
            OrderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");


            var client = new ServiceBusClient(serviceBusConnectionString);

            EmailLogsProcessor = client.CreateProcessor(OrderUpdatePaymentResultTopic, subscriptionEmailLogs);
        }
        public async Task Start()
        {
            EmailLogsProcessor.ProcessMessageAsync += OnEmailLogsReceived;
            EmailLogsProcessor.ProcessErrorAsync += ErrorHandler;
            await EmailLogsProcessor.StartProcessingAsync();

        }



        public async Task Stop()
        {
            await EmailLogsProcessor.StopProcessingAsync();
            await EmailLogsProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }



        private async Task OnEmailLogsReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage Email = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            await _emailRepository.SendAndLogEmail(Email);
            await arg.CompleteMessageAsync(message);
            
        }   
    }
}
