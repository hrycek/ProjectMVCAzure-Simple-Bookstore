using Microsoft.AspNetCore.Mvc;
using MvcProject2.Models;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNet.SignalR;
using System.Web;

namespace MvcProject2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ViewDataDictionary ViewData { get; set; }

        public ServiceBusClient client;

        // the processor that reads and processes messages from the queue
        public ServiceBusProcessor processor;

        public List<MessageClass> messages = new List<MessageClass> {
            new MessageClass
            {
                message = "Witam! Jestem stałym użytkownikiem waszego serwisu i chciałbym zadać kilka pytań odnośnie korzystania z niego." +
                "Mianowicie czy istnieje możliwość dodania książki do bazy danych czy jest to funkcja tylko dla administratora strony? Z góry dziękuję za odpowiedź :)",
                email = "example@gmail.com"
            },
            new MessageClass
            {
                message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque venenatis scelerisque erat non cursus. Donec sollicitudin pellentesque consequat. Sed vestibulum augue et est fringilla, non laoreet augue gravida. Donec ac ultricies elit, id consectetur neque.",
                email = "example@gmail.com"
            },
            new MessageClass
            {
                message = "Aliquam dignissim dolor in nulla imperdiet, posuere porta lorem iaculis. Fusce feugiat nisl sit amet tellus molestie tempor. Nam arcu turpis, varius ac neque quis, dapibus venenatis risus. Donec dapibus vitae sem sit amet facilisis.",
                email = "example@gmail.com"
            },
             new MessageClass
            {
                message = "Nulla eget nunc in ante mattis varius ac tincidunt tortor. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Quisque vitae nunc risus. Aenean porttitor, neque sed condimentum luctus, ex quam pretium ex, et dictum massa nibh ac quam.",
                email = "example@gmail.com"
            }
        };

        public MessageClass message;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("currUser"))
            {
                var currUser = HttpContext.Request.Cookies["currUser"];
                Console.WriteLine(currUser);
                ViewBag.CurrUser = currUser;
                return View();
            }
           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SupportPanel()
        { 
          
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient("Endpoint=sb://projectpchsb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=B6e5E8+yoj/GU7QcNUQWFqB/jYAHFDeH1d9P0yHWqi4=", clientOptions);
            processor = client.CreateProcessor("projectpch_bus", new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Thread.Sleep(1000);


                if (processor.IsProcessing)
                {
                    Console.WriteLine("\nStopping the receiver...");
                    processor.StopProcessingAsync();
                    Console.WriteLine("Stopped receiving messages");
                    message = new MessageClass
                    {
                        message = "brak",
                        email = "example@gmail.com"
                    };
                }
            }
            finally
            {
                processor.DisposeAsync();
                client.DisposeAsync();
            }
 
            return View(message);
        }

        public IActionResult Login()
        {

            return View();
        }

        public async Task<IActionResult> SetUserCookie(string username)
        {

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMinutes(20));
            HttpContext.Response.Cookies.Append("currUser", username, cookieOptions);
         
            return RedirectToAction("Index");
        }
       
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received2: {body}");
            string msg = body.Split("|-|")[0];
            string email = body.Split("|-|")[1];

            message = new MessageClass
            {
                message = msg,
                email = email
            };

            messages.Add(new MessageClass
            {
                message = msg,
                email = email
            });
            

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);

            ThreadPool.QueueUserWorkItem(async delegate {
                await processor.StopProcessingAsync();
            });
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}