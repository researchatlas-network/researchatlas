using Autofac;
using ResearchAtlas.Application.Abstractions.Persistence;

namespace ResearchAtlas.Infrastructure.Persistence.SqlServer.Transactions;

public sealed class EfUnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly ILifetimeScope _lifetimeScope;

    public EfUnitOfWorkFactory(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public async Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
    {
        var lifetimeScope = _lifetimeScope.BeginLifetimeScope();

        try
        {
            var unitOfWork = lifetimeScope.Resolve<EfUnitOfWork>();
            await unitOfWork.BeginTransactionAsync(lifetimeScope, cancellationToken);
            return unitOfWork;
        }
        catch
        {
            lifetimeScope.Dispose();
            throw;
        }
    }
}
