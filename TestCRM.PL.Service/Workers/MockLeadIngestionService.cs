using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Models;

namespace TestCRM.PL.Service.Workers
{
    public class MockLeadIngestionService(
        ILogger<MockLeadIngestionService> logger,
        ILeadQueue leadQueue) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var random = new Random();
                var leadsCount = random.Next(1, 5);

                for (int i = 0; i < leadsCount; i++)
                {
                    var randomNumber = random.Next(1000, 10000);

                    var leadDto = new CreateLeadDto
                    {
                        Name = $"Test Lead {randomNumber}",
                        Email = $"lead{randomNumber}@example.com",
                        Phone = $"+3800000{randomNumber}",
                    };

                    logger.LogInformation("Enqueuing lead: {Email}", leadDto.Email);
                
                    await leadQueue.EnqueueAsync(leadDto);
                }

                var delaySeconds = random.Next(10);

                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
            }
        }
    }
}
