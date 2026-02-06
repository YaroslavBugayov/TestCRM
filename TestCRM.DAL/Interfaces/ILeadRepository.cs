using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.DAL.Entities;

namespace TestCRM.DAL.Interfaces
{
    public interface ILeadRepository
    {
        Task<int> CreateLeadAsync(LeadEntity lead, CancellationToken ct = default);
        Task<int> UpdateLeadAsync(LeadEntity lead, CancellationToken ct = default);
        Task<int> DeleteLeadAsync(int id, CancellationToken ct = default);
        Task<LeadEntity?> GetLeadByIdAsync(int id);
        Task<IEnumerable<LeadEntity>> GetAllLeadsAsync();
    }
}
