using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Core;

namespace MyMvcApp.Controllers
{
   
    public class ChatController : Controller
    {
        public ServiceBusClient? client;
        public ServiceBusSender? sender;

       
        public IActionResult Chat()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(string message, string email)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            client = new ServiceBusClient("Endpoint=sb://projectpchsb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=B6e5E8+yoj/GU7QcNUQWFqB/jYAHFDeH1d9P0yHWqi4=", clientOptions);
            sender = client.CreateSender("projectpch_bus");

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            string msg = $"{message}|-|{email}";
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(msg)))
            {
                // if it is too large for the batch
                throw new Exception($"The message {msg} is too large to fit in the batch.");
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of message {msg} has been published to the queue.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

            // Redirect to the chat history
            return RedirectToAction("Chat");
        }

      
    }
}