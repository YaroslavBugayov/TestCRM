using TestCRM.BLL.Interfaces;

namespace TestCRM.PL.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILeadQueue _leadQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<Worker> _logger;

        public Worker(ILeadQueue leadQueue, IServiceScopeFactory scopeFactory, ILogger<Worker> logger)
        {
            _leadQueue = leadQueue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = stoppingToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            await Parallel.ForEachAsync(_leadQueue.DequeueAllAsync(stoppingToken), parallelOptions, async (leadDto, ct) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var leadProcessor = scope.ServiceProvider.GetRequiredService<ILeadProcessor>();
                try
                {
                    await leadProcessor.ProcessLeadAsync(leadDto, ct);
                    _logger.LogInformation("Successfully processed lead with ID {LeadId}", leadDto.Id);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Processing of lead with ID {LeadId} was canceled.", leadDto.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing lead with ID {LeadId}", leadDto.Id);
                }
            });
        }
    }
}
