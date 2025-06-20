using ResumeRanker.Data;
using ResumeRanker.Services;

namespace ResumeRanker.Background;

public class ResumeProcessingWorker(IServiceProvider services) : BackgroundService
{
    private readonly IServiceProvider _services = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var scorer = scope.ServiceProvider.GetRequiredService<ResumeScoringService>();

            var pendingJobs = db.JobRequests
                .Where(j => !j.IsProcessed)
                .ToList();

            foreach (var job in pendingJobs)
            {
                await scorer.ProcessJobAsync(job);
                job.IsProcessed = true;
                await db.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
