using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class EvaluatorEvaluationReport
{
    internal EvaluatorEvaluationReport(Guid id, Guid reportTemplateId, Person evaluator, bool isPositive, string motivation)
    {
        Id = id;
        ReportTemplateId = reportTemplateId;
        Evaluator = evaluator;
        IsPositive = isPositive;
        Motivation = motivation;
    }

    public Guid Id { get; }

    public Guid ReportTemplateId { get; }

    public Person Evaluator { get; }

    public bool IsPositive { get; }

    public string Motivation { get; }
}
