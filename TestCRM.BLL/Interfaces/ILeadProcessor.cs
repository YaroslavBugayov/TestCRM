using TestCRM.BLL.Models;

namespace TestCRM.BLL.Interfaces
{
    public interface ILeadProcessor
    {
        Task<int> ProcessLeadAsync(CreateLeadDto leadDto, CancellationToken ct = default);
    }
}
