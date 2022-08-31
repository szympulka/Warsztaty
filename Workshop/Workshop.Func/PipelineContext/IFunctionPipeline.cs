using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;
using System;

namespace Workshop.Func.PipelineContext
{
    public interface IFunctionPipeline
    {
        PipelineContext CreateContext(HttpRequest request);
        PipelineContext CreateContext(string correlationId);
        PipelineContext CreateContext(Message serviceBusMessage);
        string GetCorrelationId();
    }

    public class PipelineContext : IDisposable
    {
        private IDisposable _scope;

        internal PipelineContext(string correlationId, IDisposable scope)
        {
            CorrelationId = correlationId;
            _scope = scope;
        }

        public string CorrelationId { get; }

        void IDisposable.Dispose()
        {
            if (_scope != null)
            {
                _scope.Dispose();
                _scope = null;
            }
        }
    }
}
