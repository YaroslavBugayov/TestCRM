using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;
using TestCRM.DAL.Entities;
using TestCRM.DAL.Interfaces;

namespace TestCRM.BLL.Services
{
    public class LeadProcessor(ILeadRepository repository, ILogger<LeadProcessor> logger) : ILeadProcessor
    {

        public async Task<int> ProcessLeadAsync(CreateLeadDto leadDto, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                Validator.ValidateObject(leadDto, new ValidationContext(leadDto), validateAllProperties: true);

                var lead = MapToEntity(leadDto);
                return await repository.CreateAsync(lead, ct);
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validation failed for lead {Email}: {Message}", leadDto.Email, ex.Message);
                throw;
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Lead processing for {Email} was canceled.", leadDto.Email);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing lead {Email}", leadDto.Email);
                throw;
            }
        }

        private LeadEntity MapToEntity(CreateLeadDto leadDto)
        {
            return new LeadEntity
            {
                Name = leadDto.Name,
                Email = leadDto.Email,
                Phone = leadDto.Phone,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
