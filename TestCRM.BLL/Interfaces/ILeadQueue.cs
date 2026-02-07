using TestCRM.BLL.Models;

namespace TestCRM.BLL.Interfaces
{
    public interface ILeadQueue
    {
        ValueTask EnqueueAsync(CreateLeadDto lead, CancellationToken ct = default);
        IAsyncEnumerable<CreateLeadDto> DequeueAllAsync(CancellationToken ct = default);
    }
}
