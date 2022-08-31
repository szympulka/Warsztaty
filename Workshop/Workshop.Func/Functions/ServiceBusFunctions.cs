
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text;
using Workshop.Func.PipelineContext;

namespace Workshop.Func.Functions
{
    public class ServiceBusFunctions
    {
        private readonly IFunctionPipeline functionPipeline;

        public ServiceBusFunctions(IFunctionPipeline functionPipeline)
        {
            this.functionPipeline = functionPipeline;
        }

        [FunctionName("ServiceBusFunctions")]
        public void Run([ServiceBusTrigger("queue", Connection = "sbConnectionString")] Message message, ILogger log)
        {
            using var pipeline = functionPipeline.CreateContext(message);
            log.LogInformation($"C# ServiceBus queue trigger function processed message");
            var body = Encoding.UTF8.GetString(message.Body);
        }

        [FunctionName("DeadLetterQueue")]
        public void DeadLetterQueue([ServiceBusTrigger("queue/$DeadLetterQueue", Connection = "sbConnectionString")] Message myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message");
        }
    }
}
