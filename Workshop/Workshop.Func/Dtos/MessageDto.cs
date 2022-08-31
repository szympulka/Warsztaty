using System.Collections.Generic;

namespace Workshop.Func.Dtos
{
    public record MessageDto
    {
        public string CorrelationId { get; init; }
        public List<int> Data { get; init; }
    }
}
