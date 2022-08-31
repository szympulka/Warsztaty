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
            this.queueClient = new QueueClient("", "queue", new QueueClientOptions
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
