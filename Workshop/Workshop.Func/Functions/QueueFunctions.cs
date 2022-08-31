using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using Workshop.Func.Dtos;
using Workshop.Func.PipelineContext;
using Workshop.Func.Services;

namespace Workshop.Func.Functions
{
    public class QueueFunctions
    {
        private readonly IFunctionPipeline functionPipeline;
        private readonly IServiceBusService serviceBusService;

        public QueueFunctions(IFunctionPipeline functionPipeline, IServiceBusService serviceBusService)
        {
            this.functionPipeline = functionPipeline;
            this.serviceBusService = serviceBusService;
        }


        [FunctionName("QueueFunctions")]
        public void Run([QueueTrigger("queue", Connection = "storageConnetionString")] string myQueueItem, ILogger log)
        {
            var data = JsonConvert.DeserializeObject<MessageDto>(myQueueItem);
            using var pipeline = functionPipeline.CreateContext(data.CorrelationId);
            log.LogInformation("Getting message for QueueFunctions");

            for (int i = 0; i < data.Data.Count; i++)
            {
                data.Data[i] += new Random().Next(3);
            }
            try
            {
                serviceBusService.SendMessage("queue", JsonConvert.SerializeObject(data.Data.OrderByDescending(x => x).ToList()));
            }
            catch (System.Exception)
            {
                log.LogError($"Can't send message for data {data}");
                throw;
            }

        }
    }
}
