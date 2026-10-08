namespace ResearchAtlas.Application.Abstractions.Jobs;

public interface IJobClient
{
    Task<string> EnqueueAsync<TJob>(TJob job, CancellationToken cancellationToken = default);
}
