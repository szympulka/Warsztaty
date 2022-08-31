using System;
using System.Collections.Generic;
using Workshop.Func.Dtos;
using Workshop.Func.PipelineContext;

namespace Workshop.Func.Services
{
    public interface IDataService
    {
        MessageDto GetData();
    }
    public class DataService : IDataService
    {
        private readonly IFunctionPipeline functionPipeline;

        public DataService(IFunctionPipeline functionPipeline)
        {
            this.functionPipeline = functionPipeline;
        }

        public MessageDto GetData()
        {
            var correlationId = functionPipeline.GetCorrelationId();

            var list = new List<int>();
            list.Add(new Random().Next(-10, 50));
            list.Add(new Random().Next(-10, 50));
            list.Add(new Random().Next(-10, 50));
            list.Add(new Random().Next(-10, 50));

            return new MessageDto()
            {
                CorrelationId = correlationId,
                Data = list,
            };

        }
    }
}
