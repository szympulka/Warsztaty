using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace Workshop.Func.Services
{
    public interface IStorageService
    {
        void SendMessage(string message);
    }
    public class StorageService : IStorageService
    {
        private readonly QueueClient queueClient;
        private readonly ILogger<StorageService> logger;

        public StorageService(ILogger<StorageService> logger)
        {
            this.queueClient = new QueueClient("DefaultEndpointsProtocol=https;AccountName=stqwerweu001d;AccountKey=InVsjtqOfqxZ8xxvZ2BRU4oz2VVpM0vNZjNofOgrfMEGSibG0NyfSS5tJAQjdwYd8IFllJVQUUtD+AStvvYEiw==;EndpointSuffix=core.windows.net", "queue", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.None
            });
            this.logger = logger;
        }

        public void SendMessage(string message)
        {
            logger.LogInformation("Try create queue if not exist");
            queueClient.CreateIfNotExists();

            logger.LogInformation("Try send message");
            try
            {
                queueClient.SendMessage(message);
            }
            catch (System.Exception)
            {
                logger.LogError($"error for message {message}");
                throw;
            }
        }
    }
}
