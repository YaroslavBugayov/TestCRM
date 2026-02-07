using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;

namespace TestCRM.BLL.Services
{
    public class LeadQueue(ILogger<LeadQueue> logger) : ILeadQueue
    {
        private readonly Channel<CreateLeadDto> _channel = Channel.CreateUnbounded<CreateLeadDto>();

        public IAsyncEnumerable<CreateLeadDto> DequeueAllAsync(CancellationToken ct)
        {
            return _channel.Reader.ReadAllAsync(ct);
        }

        public async ValueTask EnqueueAsync(CreateLeadDto lead, CancellationToken ct)
        {
            await _channel.Writer.WriteAsync(lead, ct);
            logger.LogInformation("Lead enqueued: {Email}. Currenty in queue {Count} leads", lead.Email, _channel.Reader.Count);
        }
    }
}
