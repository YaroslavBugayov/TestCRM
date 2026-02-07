using TestCRM.BLL.Interfaces;

namespace TestCRM.PL.Service.Workers
{
    public class LeadProcessingWorker(
        ILeadQueue leadQueue,
        IServiceScopeFactory scopeFactory,
        ILogger<LeadProcessingWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = stoppingToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            await Parallel.ForEachAsync(leadQueue.DequeueAllAsync(stoppingToken), parallelOptions, async (leadDto, ct) =>
            {
                using var scope = scopeFactory.CreateScope();
                var leadProcessor = scope.ServiceProvider.GetRequiredService<ILeadProcessor>();
                try
                {
                    var id = await leadProcessor.ProcessLeadAsync(leadDto, ct);
                    logger.LogInformation("Successfully processed lead {Email}", leadDto.Email);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Processing of lead {Email} was canceled.", leadDto.Email);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing lead {Email}", leadDto.Email);
                }
            });
        }
    }
}
