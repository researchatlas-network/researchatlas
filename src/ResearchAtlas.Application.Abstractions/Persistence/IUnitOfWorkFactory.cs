namespace ResearchAtlas.Application.Abstractions.Persistence;

public interface IUnitOfWorkFactory
{
    Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
}
