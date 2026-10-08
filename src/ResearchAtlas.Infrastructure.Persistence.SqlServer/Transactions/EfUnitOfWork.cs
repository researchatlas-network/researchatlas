using Autofac;
using Microsoft.EntityFrameworkCore.Storage;
using ResearchAtlas.Application.Abstractions.Persistence;
using ResearchAtlas.Infrastructure.Persistence.SqlServer.Context;

namespace ResearchAtlas.Infrastructure.Persistence.SqlServer.Transactions;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ResearchAtlasDbContext _dbContext;
    private ILifetimeScope? _lifetimeScope;
    private IDbContextTransaction? _transaction;
    private bool _isCompleted;

    public EfUnitOfWork(ResearchAtlasDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    internal async Task BeginTransactionAsync(
        ILifetimeScope lifetimeScope,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(lifetimeScope);

        if (_lifetimeScope is not null)
        {
            throw new InvalidOperationException("A transaction is already active for this unit of work.");
        }

        _lifetimeScope = lifetimeScope;
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var transaction = GetActiveTransaction();

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            _isCompleted = true;
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        var transaction = GetActiveTransaction();

        try
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            _dbContext.ChangeTracker.Clear();
            _isCompleted = true;
            await DisposeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_transaction is { } transaction)
            {
                _transaction = null;

                try
                {
                    if (!_isCompleted)
                    {
                        await transaction.RollbackAsync();
                        _dbContext.ChangeTracker.Clear();
                    }
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            }
        }
        finally
        {
            if (_lifetimeScope is { } lifetimeScope)
            {
                _lifetimeScope = null;
                lifetimeScope.Dispose();
            }
        }
    }

    private IDbContextTransaction GetActiveTransaction()
    {
        return _transaction ?? throw new InvalidOperationException(
            "This unit of work does not have an active transaction.");
    }
}
