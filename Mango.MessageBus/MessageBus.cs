using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string connetionString = "Endpoint=sb://mangorestaurantes.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=j81Uk7rW/Vgh7a4gHpi+vNvlWyIcnKBi8tBJCFMHdzs=";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connetionString);
            ServiceBusSender serviceBus = client.CreateSender(topicName);
            var JsonMessage=JsonConvert.SerializeObject(message);
            var finalMessage=new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonMessage))
            {
                CorrelationId=Guid.NewGuid().ToString()
            };
            await serviceBus.SendMessageAsync(finalMessage);
            await  client.DisposeAsync();
        }
    }
}
