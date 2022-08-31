using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Workshop.Func.PipelineContext;
using Workshop.Func.Services;

[assembly: FunctionsStartup(typeof(Workshop.Func.Startup))]
namespace Workshop.Func
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddSingleton<IFunctionPipeline, FunctionPipeline>();
            builder.Services.AddSingleton<IStorageService, StorageService>();
            builder.Services.AddSingleton<IServiceBusService, ServiceBusService>();
        }
    }
}
