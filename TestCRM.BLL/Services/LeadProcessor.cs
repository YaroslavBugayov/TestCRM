using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;
using TestCRM.DAL.Entities;
using TestCRM.DAL.Interfaces;

namespace TestCRM.BLL.Services
{
    public class LeadProcessor(ILeadRepository repository, ILogger<LeadProcessor> logger) : ILeadProcessor
    {
        private readonly ILeadRepository _repository = repository;
        private readonly ILogger<LeadProcessor> _logger = logger;

        public Task ProcessLeadAsync(LeadDto leadDto, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                if (!IsValidLead(leadDto))
                {
                    _logger.LogWarning("Lead with ID {LeadId} failed validation and will not be processed.", leadDto.Id);
                    return Task.CompletedTask;
                }

                var lead = MapToEntity(leadDto);
                return _repository.CreateLeadAsync(lead, ct);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Lead processing for ID {LeadId} was canceled.", leadDto.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing lead with ID {LeadId}", leadDto.Id);
                throw;
            }
        }

        private LeadEntity MapToEntity(LeadDto leadDto)
        {
            return new LeadEntity
            {
                Id = leadDto.Id,
                Name = leadDto.Name,
                Email = leadDto.Email,
                Phone = leadDto.Phone,
                CreatedAt = leadDto.CreatedAt
            };
        }

        private bool IsValidLead(LeadDto lead)
        {
            if (string.IsNullOrWhiteSpace(lead.Name))
            {
                _logger.LogWarning("Lead validation failed: Name is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(lead.Email))
            {
                _logger.LogWarning("Lead validation failed: Email is required.");
                return false;
            }
            if (!IsValidEmail(lead.Email))
            {
                _logger.LogWarning("Lead validation failed: Invalid email format for {Email}.", lead.Email);
                return false;
            }
            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
