using ResearchAtlas.Domain.Abstractions.Events;

namespace ResearchAtlas.Infrastructure.Messaging;

public sealed class LifetimeScopeAdapter : ILifetimeScope
{
    private readonly IServiceProvider _serviceProvider;

    public LifetimeScopeAdapter(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public TService Resolve<TService>()
    {
        var service = _serviceProvider.GetService(typeof(TService));

        if (service is null)
            throw new InvalidOperationException($"Service of type '{typeof(TService).FullName}' is not registered.");

        return (TService)service;
    }
}
