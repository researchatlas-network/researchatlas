using ResearchAtlas.Domain.Abstractions.Events;

namespace ResearchAtlas.Infrastructure.Messaging
{
    public sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILifetimeScope _scope;

        public DomainEventDispatcher(ILifetimeScope scope) => _scope = scope;

        public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
        {
            foreach (var e in events)
                await DispatchOneAsync((dynamic)e, ct);
        }

        private async Task DispatchOneAsync<TEvent>(TEvent @event, CancellationToken ct)
            where TEvent : IDomainEvent
        {
            var handlers = _scope.Resolve<IEnumerable<IDomainEventHandler<TEvent>>>();

            foreach (var h in handlers)
                await h.HandleAsync(@event, ct);
        }
    }
}
