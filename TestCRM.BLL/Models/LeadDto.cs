using System;
using System.Collections.Generic;
using System.Text;

namespace TestCRM.BLL.Models
{
    public class LeadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
