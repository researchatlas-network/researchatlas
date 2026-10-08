using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class FinalEvaluationReport
{
    internal FinalEvaluationReport(Guid id, Guid reportTemplateId, bool isPositive, string motivation, Person committeeChair, Person committeeSecretary)
    {
        Id = id;
        ReportTemplateId = reportTemplateId;
        IsPositive = isPositive;
        Motivation = motivation;
        CommitteeChair = committeeChair;
        CommitteeSecretary = committeeSecretary;
    }

    public Guid Id { get; }

    public Guid ReportTemplateId { get; }

    public bool IsPositive { get; }

    public string Motivation { get; }

    public Person CommitteeChair { get; }

    public Person CommitteeSecretary { get; }
}
