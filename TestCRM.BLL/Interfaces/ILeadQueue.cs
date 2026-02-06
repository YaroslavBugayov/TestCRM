using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.BLL.Models;

namespace TestCRM.BLL.Interfaces
{
    public interface ILeadQueue
    {
        ValueTask EnqueueAsync(LeadDto lead, CancellationToken ct = default);
        IAsyncEnumerable<LeadDto> DequeueAllAsync(CancellationToken ct = default);
    }
}
