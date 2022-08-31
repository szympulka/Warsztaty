using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Workshop.Func.PipelineContext;

namespace Workshop.Func.Services
{
    public interface IServiceBusService
    {
        Task SendMessage(string queue, string message);
    }
    public class ServiceBusService : IServiceBusService
    {
        private readonly ServiceBusClient serviceBusClient;
        private readonly IFunctionPipeline functionPipeline;
        private readonly ILogger<ServiceBusService> logger;

        public ServiceBusService(IFunctionPipeline functionPipeline, ILogger<ServiceBusService> logger)
        {
            serviceBusClient = new ServiceBusClient("");
            this.functionPipeline = functionPipeline;
            this.logger = logger;
        }

        public async Task SendMessage(string queue, string message)
        {
            var serviceBusMessage = new ServiceBusMessage(message);
            logger.LogInformation("Create service bus sender");
            var sender = serviceBusClient.CreateSender(queue);

            logger.LogInformation("Get correlation id");
            var correlationID = functionPipeline.GetCorrelationId();
            if (correlationID != null)
            {
                logger.LogInformation($"Correlation id exist! value: {correlationID}");
                serviceBusMessage.CorrelationId = correlationID;
            }
            else
                logger.LogWarning("Correlation id not found");

            logger.LogInformation("Try send message to service bus");
            await sender.SendMessageAsync(serviceBusMessage);
            await sender.DisposeAsync();
            logger.LogInformation("Dispose connetion");
        }
    }
}
