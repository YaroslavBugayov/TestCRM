using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;

namespace TestCRM.BLL.Services
{
    public class LeadQueue : ILeadQueue
    {
        private readonly Channel<LeadDto> _channel = Channel.CreateUnbounded<LeadDto>();

        public IAsyncEnumerable<LeadDto> DequeueAllAsync(CancellationToken ct)
        {
            return _channel.Reader.ReadAllAsync(ct);
        }

        public async ValueTask EnqueueAsync(LeadDto lead, CancellationToken ct)
        {
            await _channel.Writer.WriteAsync(lead, ct);
        }
    }
}
