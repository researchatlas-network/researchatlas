using System;
using System.Collections.Generic;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
    }
}
