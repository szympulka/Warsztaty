using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Workshop.Func.PipelineContext;
using Workshop.Func.Services;

namespace Workshop.Func.Functions;

public class HttpFunctions
{
    private readonly IFunctionPipeline functionPipeline;
    private readonly IDataService dataService;
    private readonly IStorageService storageService;

    public HttpFunctions(IFunctionPipeline functionPipeline, IDataService dataService, IStorageService storageService)
    {
        this.functionPipeline = functionPipeline;
        this.dataService = dataService;
        this.storageService = storageService;
    }

    [FunctionName("HttpFunctions")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        using var context = functionPipeline.CreateContext(req);

        log.LogInformation("Run HttpFunctions");
        var data = dataService.GetData();
        log.LogInformation("Getting data");
        var json = JsonConvert.SerializeObject(data);
        log.LogInformation($"Data: {json}");

        storageService.SendMessage(json);


        return new OkObjectResult(json);
    }
}
