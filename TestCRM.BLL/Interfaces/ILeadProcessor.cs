using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.BLL.Models;

namespace TestCRM.BLL.Interfaces
{
    public interface ILeadProcessor
    {
        Task<int> ProcessLeadAsync(LeadDto leadDto, CancellationToken ct = default);
    }
}
