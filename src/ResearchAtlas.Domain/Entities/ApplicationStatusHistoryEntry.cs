using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class ApplicationStatusHistoryEntry
{
    internal ApplicationStatusHistoryEntry(
        Guid id,
        Enums.ApplicationStatus status,
        DateTimeOffset occurredAtUtc,
        User changedBy)
    {
        Id = id;
        Status = status;
        OccurredAtUtc = occurredAtUtc;
        ChangedBy = changedBy;
    }

    public Guid Id { get; }

    public Enums.ApplicationStatus Status { get; }

    public DateTimeOffset OccurredAtUtc { get; }

    public User ChangedBy { get; }

}
