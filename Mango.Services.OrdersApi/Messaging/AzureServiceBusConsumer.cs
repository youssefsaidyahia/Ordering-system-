using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.OrdersApi.Messages;
using Mango.Services.OrdersApi.Models;
using Mango.Services.OrdersApi.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrdersApi.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;

        private readonly string serviceBusConnectionString;
        private readonly string subscriptionCheckOut;
        private readonly string checkoutMessageTopic;
        private readonly string OrderPaymentProcessTopic;
        private readonly string OrderUpdatePaymentResultTopic;

        private ServiceBusProcessor checkOutProcessor;
        private ServiceBusProcessor OrderUpdatePayment;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            _messageBus = messageBus;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            OrderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");
            OrderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");


            var client = new ServiceBusClient(serviceBusConnectionString);

            checkOutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckOut);
            OrderUpdatePayment = client.CreateProcessor(OrderUpdatePaymentResultTopic, subscriptionCheckOut);
        }
        public async Task Start()
        {
            checkOutProcessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            checkOutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkOutProcessor.StartProcessingAsync();

            OrderUpdatePayment.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            OrderUpdatePayment.ProcessErrorAsync += ErrorHandler;
            await OrderUpdatePayment.StartProcessingAsync();
        }



        public async Task Stop()
        {
            await checkOutProcessor.StopProcessingAsync();
            await checkOutProcessor.DisposeAsync();

            await OrderUpdatePayment.StopProcessingAsync();
            await OrderUpdatePayment.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                orderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickupDateTime = checkoutHeaderDto.PickupDateTime
            };
            foreach (var detailList in checkoutHeaderDto.cartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.orderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrder(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email
             
            };

            try
            {
                await _messageBus.PublishMessage(paymentRequestMessage, OrderPaymentProcessTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception e) 
            {
                throw;
            }
        }

        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage payment = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            await _orderRepository.UpdateOrderPaymentStatus(payment.OrderId, payment.status);
            await arg.CompleteMessageAsync(message);
            
        }   
    }
}
