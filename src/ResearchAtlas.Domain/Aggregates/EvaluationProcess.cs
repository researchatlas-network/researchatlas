using ResearchAtlas.Domain.Entities;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class EvaluationProcess
{
    private readonly List<EvaluatorEvaluationReport> _evaluatorReports = [];

    internal EvaluationProcess(
        Guid id,
        Application application,
        Committee committee,
        EvaluationProcessType type,
        EvaluationProcess? precedingProcess,
        User startedBy,
        DateTimeOffset startedAtUtc)
    {
        Id = id;
        Application = application;
        Committee = committee;
        Type = type;
        PrecedingProcess = precedingProcess;
        StartedBy = startedBy;
        StartedAtUtc = startedAtUtc;
        Status = EvaluationProcessStatus.InProgress;
    }

    public Guid Id { get; }

    public Application Application { get; }

    public Committee Committee { get; }

    public EvaluationProcessType Type { get; }

    public EvaluationProcess? PrecedingProcess { get; }

    public EvaluationProcessStatus Status { get; private set; }

    public EvaluationProcessOutcome? Outcome { get; private set; }

    public User StartedBy { get; }

    public DateTimeOffset StartedAtUtc { get; }

    public User? CompletedBy { get; private set; }

    public DateTimeOffset? CompletedAtUtc { get; private set; }

    public IReadOnlyCollection<EvaluatorEvaluationReport> EvaluatorReports => _evaluatorReports.AsReadOnly();

    public FinalEvaluationReport? FinalReport { get; private set; }

    public void AddEvaluatorReport(Guid id, Guid reportTemplateId, Person evaluator, bool isPositive, string motivation)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(reportTemplateId, Guid.Empty);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentException.ThrowIfNullOrWhiteSpace(motivation);
        EnsureInProgress();

        if (!evaluator.HasSignedEvaluatorCodeOfEthics || !evaluator.IsApprovedForEvaluatorCollaboration)
        {
            throw new ArgumentException("The evaluator must sign the evaluator code of ethics and be approved for evaluator collaboration before submitting a report.", nameof(evaluator));
        }

        if (_evaluatorReports.Any(report => report.Evaluator.Id == evaluator.Id))
        {
            throw new ArgumentException("The evaluator already has a report for this evaluation process.", nameof(evaluator));
        }

        _evaluatorReports.Add(new EvaluatorEvaluationReport(id, reportTemplateId, evaluator, isPositive, motivation.Trim()));
    }

    public void SetFinalReport(Guid id, Guid reportTemplateId, bool isPositive, string motivation)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(reportTemplateId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(motivation);
        EnsureInProgress();

        if (FinalReport is not null)
        {
            throw new InvalidOperationException("The evaluation process already has a final report.");
        }

        FinalReport = new FinalEvaluationReport(
            id,
            reportTemplateId,
            isPositive,
            motivation.Trim(),
            Committee.Chair.Person,
            Committee.Secretary.Person);
    }

    public void Complete(EvaluationProcessOutcome outcome, DateTimeOffset completedAtUtc, User completedBy)
    {
        ArgumentNullException.ThrowIfNull(completedBy);

        EnsureInProgress();

        if (!Enum.IsDefined(outcome))
        {
            throw new ArgumentOutOfRangeException(nameof(outcome));
        }

        if (completedAtUtc < StartedAtUtc)
        {
            throw new ArgumentOutOfRangeException(nameof(completedAtUtc));
        }

        if (Type == EvaluationProcessType.AppealReview && outcome is not (EvaluationProcessOutcome.AppealAccepted or EvaluationProcessOutcome.AppealRejected))
        {
            throw new ArgumentException("An appeal review must result in an appeal outcome.", nameof(outcome));
        }

        if (Type != EvaluationProcessType.AppealReview && outcome is not (EvaluationProcessOutcome.Positive or EvaluationProcessOutcome.Negative))
        {
            throw new ArgumentException("An evaluation process must result in a positive or negative outcome.", nameof(outcome));
        }

        Outcome = outcome;
        CompletedBy = completedBy;
        CompletedAtUtc = completedAtUtc;
        Status = EvaluationProcessStatus.Completed;
    }

    public bool CanStartReevaluation()
    {
        return Type == EvaluationProcessType.AppealReview
            && Status == EvaluationProcessStatus.Completed
            && Outcome == EvaluationProcessOutcome.AppealAccepted;
    }

    private void EnsureInProgress()
    {
        if (Status != EvaluationProcessStatus.InProgress)
        {
            throw new InvalidOperationException("Only an in-progress evaluation process can be modified.");
        }
    }
}
