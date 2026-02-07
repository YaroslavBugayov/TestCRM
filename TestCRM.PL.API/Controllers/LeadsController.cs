using Microsoft.AspNetCore.Mvc;
using TestCRM.BLL.Interfaces;
using TestCRM.DAL.Interfaces;

namespace TestCRM.PL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadQueue _leadQueue;
        private readonly ILeadRepository _leadRepository;

        public LeadsController(ILeadQueue leadQueue, ILeadRepository leadRepository)
        {
            _leadQueue = leadQueue;
            _leadRepository = leadRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeads([FromQuery] int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            try
            {
                var response = await _leadRepository.GetPagedAsync(pageNumber, pageSize, ct);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return BadRequest("Request was cancelled.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
