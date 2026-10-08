using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class AuditEventFactory
{
    public static AuditEvent Create(
        Guid id,
        User actor,
        Enums.AuditAction action,
        string affectedAggregateType,
        Guid affectedAggregateId,
        string? details)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(actor);

        if (!Enum.IsDefined(action))
        {
            throw new ArgumentOutOfRangeException(nameof(action));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(affectedAggregateType);
        ArgumentOutOfRangeException.ThrowIfEqual(affectedAggregateId, Guid.Empty);

        return new AuditEvent(
            id,
            actor,
            action,
            DateTimeOffset.UtcNow,
            affectedAggregateType.Trim(),
            affectedAggregateId,
            string.IsNullOrWhiteSpace(details) ? null : details.Trim());
    }
}
