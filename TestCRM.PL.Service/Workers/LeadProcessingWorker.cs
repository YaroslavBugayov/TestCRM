using TestCRM.BLL.Interfaces;

namespace TestCRM.PL.Service.Workers
{
    public class LeadProcessingWorker : BackgroundService
    {
        private readonly ILeadQueue _leadQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LeadProcessingWorker> _logger;

        public LeadProcessingWorker(ILeadQueue leadQueue, IServiceScopeFactory scopeFactory, ILogger<LeadProcessingWorker> logger)
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
                    var id = await leadProcessor.ProcessLeadAsync(leadDto, ct);
                    _logger.LogInformation("Successfully processed lead {Email}", leadDto.Email);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Processing of lead {Email} was canceled.", leadDto.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing lead {Email}", leadDto.Email);
                }
            });
        }
    }
}
