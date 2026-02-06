using System;
using System.Collections.Generic;
using System.Text;
using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;

namespace TestCRM.PL.Service.Workers
{
    public class MockLeadIngestionService : BackgroundService
    {
        private readonly ILogger<MockLeadIngestionService> _logger;
        private readonly ILeadQueue _leadQueue;

        public MockLeadIngestionService(ILogger<MockLeadIngestionService> logger, ILeadQueue leadQueue)
        {
            _logger = logger;
            _leadQueue = leadQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var random = new Random();
                var leadsCount = random.Next(1, 5);

                for (int i = 0; i < leadsCount; i++)
                {
                    var randomNumber = random.Next(1000, 10000);

                    var leadDto = new LeadDto
                    {
                        Name = $"Test Lead {randomNumber}",
                        Email = $"lead{randomNumber}@example.com",
                        Phone = $"+3800000{randomNumber}",
                        CreatedAt = DateTime.UtcNow
                    };

                    _logger.LogInformation("Enqueuing lead: {Email}", leadDto.Email);
                
                    await _leadQueue.EnqueueAsync(leadDto);
                }

                var delaySeconds = random.Next(10);

                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
            }
        }
    }
}
