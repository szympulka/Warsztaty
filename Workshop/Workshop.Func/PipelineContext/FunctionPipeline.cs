using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Workshop.Func.PipelineContext
{
    public class FunctionPipeline : IFunctionPipeline
    {
        public const string DefaultHeader = "X-Correlation-ID";
        private readonly ILogger<FunctionPipeline> logger;
        private string _correlationId = string.Empty;

        public FunctionPipeline(ILogger<FunctionPipeline> logger)
        {
            this.logger = logger;
        }

        public PipelineContext CreateContext(HttpRequest request)
        {
            var hasCorrelationIdHeader =
                request.Headers.TryGetValue(DefaultHeader, out var cid) && !StringValues.IsNullOrEmpty(cid);


            if (!hasCorrelationIdHeader)
            {
                cid = new StringValues(Guid.NewGuid().ToString());
            }

            _correlationId = cid;
            request.HttpContext.Response.Headers.Add(DefaultHeader, cid);

            var scope = logger.BeginScope(new Dictionary<string, object> { { "CorrelationId", cid.ToString() } });
            var context = new PipelineContext(cid, scope);

            return context;
        }

        public PipelineContext CreateContext(string correlationId)
        {
            var scope = logger.BeginScope(new Dictionary<string, object> { { "CorrelationId", correlationId } });
            var context = new PipelineContext(correlationId, scope);

            return context;
        }

        public PipelineContext CreateContext(Message serviceBusMessage)
        {
            var cid = serviceBusMessage.CorrelationId;

            if (string.IsNullOrEmpty(cid))
                cid = Guid.NewGuid().ToString();

            var scope = logger.BeginScope(new Dictionary<string, object> { { "CorrelationId", cid } });

            var context = new PipelineContext(cid, scope);

            return context;
        }

        public string GetCorrelationId() => _correlationId;
    }
}
