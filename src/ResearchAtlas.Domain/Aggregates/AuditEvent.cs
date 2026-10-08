namespace ResearchAtlas.Domain.Aggregates;

public sealed class AuditEvent
{
    internal AuditEvent(
        Guid id,
        User actor,
        Enums.AuditAction action,
        DateTimeOffset occurredAtUtc,
        string affectedAggregateType,
        Guid affectedAggregateId,
        string? details)
    {
        Id = id;
        Actor = actor;
        Action = action;
        OccurredAtUtc = occurredAtUtc;
        AffectedAggregateType = affectedAggregateType;
        AffectedAggregateId = affectedAggregateId;
        Details = details;
    }

    public Guid Id { get; }

    public User Actor { get; }

    public Enums.AuditAction Action { get; }

    public DateTimeOffset OccurredAtUtc { get; }

    public string AffectedAggregateType { get; }

    public Guid AffectedAggregateId { get; }

    public string? Details { get; }
}
