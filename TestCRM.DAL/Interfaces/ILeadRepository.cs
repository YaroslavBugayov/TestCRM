using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.DAL.Entities;

namespace TestCRM.DAL.Interfaces
{
    public interface ILeadRepository
    {
        Task<int> CreateLeadAsync(LeadEntity lead);
        Task<int> UpdateLeadAsync(LeadEntity lead);
        Task<int> DeleteLeadAsync(int id);
        Task<LeadEntity?> GetLeadByIdAsync(int id);
        Task<IEnumerable<LeadEntity>> GetAllLeadsAsync();
    }
}
