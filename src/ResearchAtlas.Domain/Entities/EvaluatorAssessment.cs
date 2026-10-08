using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class EvaluatorAssessment
{
    internal EvaluatorAssessment(
        Guid id,
        bool shouldRepeatAsEvaluator,
        string comments,
        User assessedBy,
        DateTimeOffset assessedAtUtc)
    {
        Id = id;
        ShouldRepeatAsEvaluator = shouldRepeatAsEvaluator;
        Comments = comments;
        AssessedBy = assessedBy;
        AssessedAtUtc = assessedAtUtc;
    }

    public Guid Id { get; }

    public bool ShouldRepeatAsEvaluator { get; }

    public string Comments { get; }

    public User AssessedBy { get; }

    public DateTimeOffset AssessedAtUtc { get; }
}
