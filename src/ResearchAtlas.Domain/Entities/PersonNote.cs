using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class PersonNote
{
    internal PersonNote(Guid id, string content, bool isCritical, User addedBy, DateTimeOffset addedAtUtc)
    {
        Id = id;
        Content = content;
        IsCritical = isCritical;
        AddedBy = addedBy;
        AddedAtUtc = addedAtUtc;
    }

    public Guid Id { get; }

    public string Content { get; }

    public bool IsCritical { get; }

    public User AddedBy { get; }

    public DateTimeOffset AddedAtUtc { get; }
}
