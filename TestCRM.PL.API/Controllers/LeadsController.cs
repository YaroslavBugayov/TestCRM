using Microsoft.AspNetCore.Mvc;
using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;
using TestCRM.DAL.Interfaces;

namespace TestCRM.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController(
        ILeadQueue leadQueue, 
        ILeadRepository leadRepository, 
        ILogger<LeadsController> logger) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetLeads([FromQuery] int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            try
            {
                var response = await leadRepository.GetPagedAsync(pageNumber, pageSize, ct);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] CreateLeadDto leadDto, CancellationToken ct = default)
        {
            try
            {
                await leadQueue.EnqueueAsync(leadDto, ct);
                return Accepted("Lead has been queued for processing.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while enqueuing lead.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
