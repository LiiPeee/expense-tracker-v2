namespace ExpenseTrackerV2.Worker;

public class AtualizacaoDeSaldoWorker : BackgroundService
{
    private readonly ILogger<AtualizacaoDeSaldoWorker> _logger;

    public AtualizacaoDeSaldoWorker(ILogger<AtualizacaoDeSaldoWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            Console.WriteLine("Passei aqui");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
