using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.DAL.Entities;

namespace TestCRM.DAL.Interfaces
{
    public interface ILeadRepository
    {
        Task<int> CreateAsync(LeadEntity lead, CancellationToken ct = default);
        Task<int> UpdateAsync(LeadEntity lead, CancellationToken ct = default);
        Task<int> DeleteAsync(int id, CancellationToken ct = default);
        Task<LeadEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<LeadEntity>> GetAllAsync(CancellationToken ct = default);
        Task<IEnumerable<LeadEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
