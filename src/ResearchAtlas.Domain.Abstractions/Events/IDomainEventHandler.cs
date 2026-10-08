using System;
using System.Collections.Generic;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Events
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken ct = default);
    }
}
