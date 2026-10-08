using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Entities;
using ResearchAtlas.Domain.Enums;
using ResearchAtlas.Domain.Rules;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class Application
{
    private readonly List<ApplicationStatusHistoryEntry> _statusHistory = [];

    internal Application(Guid id, CallForApplications callForApplications, Person applicant, User createdBy, DateTimeOffset createdAtUtc)
    {
        Id = id;
        CallForApplications = callForApplications;
        Applicant = applicant;
        _statusHistory.Add(new ApplicationStatusHistoryEntry(Guid.CreateVersion7(), ApplicationStatus.Draft, createdAtUtc, createdBy));
    }

    public Guid Id { get; }

    public CallForApplications CallForApplications { get; }

    public Person Applicant { get; }

    public Person? Reviewer { get; private set; }

    public ApplicationStatus Status => _statusHistory[^1].Status;

    public IReadOnlyCollection<ApplicationStatusHistoryEntry> StatusHistory => _statusHistory.AsReadOnly();

    public void AssignReviewer(Person reviewer)
    {
        ArgumentNullException.ThrowIfNull(reviewer);

        Reviewer = reviewer;
    }

    public void Submit(DateTimeOffset submittedAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(
            new ApplicationCanOnlyBeSubmittedWhenDraftRule(Status),
            new ApplicationCanOnlyBeSubmittedWhenCallIsOpenRule(CallForApplications.IsOpenForApplications),
            new ApplicationMustBeSubmittedDuringCallPeriodRule(
                submittedAtUtc,
                CallForApplications.ApplicationPeriodStartsAtUtc,
                CallForApplications.ApplicationPeriodEndsAtUtc));

        ChangeStatus(ApplicationStatus.Submitted, submittedAtUtc, changedBy);
    }

    public void StartEligibilityReview(DateTimeOffset startedAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationEligibilityCanOnlyBeReviewedAfterSubmissionRule(Status));

        ChangeStatus(ApplicationStatus.UnderEligibilityReview, startedAtUtc, changedBy);
    }

    public void Exclude(string reason, DateTimeOffset excludedAtUtc, User changedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationEligibilityMustBeUnderReviewRule(Status));

        ChangeStatus(ApplicationStatus.Excluded, excludedAtUtc, changedBy);
    }

    public void ConfirmEligibility(DateTimeOffset confirmedAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationEligibilityMustBeUnderReviewRule(Status));

        ChangeStatus(ApplicationStatus.Eligible, confirmedAtUtc, changedBy);
    }

    public void StartReview(DateTimeOffset startedAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationReviewRequiresEligibilityRule(Status));

        ChangeStatus(ApplicationStatus.UnderReview, startedAtUtc, changedBy);
    }

    public void Approve(DateTimeOffset occurredAtUtc, User changedBy)
    {
        RecordResolution(ApplicationStatus.Positive, occurredAtUtc, changedBy);
    }

    public void Reject(DateTimeOffset occurredAtUtc, User changedBy)
    {
        RecordResolution(ApplicationStatus.Negative, occurredAtUtc, changedBy);
    }

    public void CloseWithoutResolution(DateTimeOffset occurredAtUtc, User changedBy)
    {
        RecordResolution(ApplicationStatus.ClosedWithoutResolution, occurredAtUtc, changedBy);
    }

    public void AcceptAppeal(DateTimeOffset occurredAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationAppealDecisionRequiresResolutionRule(Status));

        CallForApplications.EnsureResolutionTemplateConfigured(ApplicationStatus.AppealAccepted);
        ChangeStatus(ApplicationStatus.AppealAccepted, occurredAtUtc, changedBy);
    }

    public void RejectAppeal(DateTimeOffset occurredAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationAppealDecisionRequiresResolutionRule(Status));

        CallForApplications.EnsureResolutionTemplateConfigured(ApplicationStatus.AppealRejected);
        ChangeStatus(ApplicationStatus.AppealRejected, occurredAtUtc, changedBy);
    }

    private void RecordResolution(ApplicationStatus status, DateTimeOffset occurredAtUtc, User changedBy)
    {
        ArgumentNullException.ThrowIfNull(changedBy);

        BusinessRuleValidator.Validate(new ApplicationResolutionRequiresReviewRule(Status));

        CallForApplications.EnsureResolutionTemplateConfigured(status);
        ChangeStatus(status, occurredAtUtc, changedBy);
    }

    private void ChangeStatus(ApplicationStatus status, DateTimeOffset occurredAtUtc, User changedBy)
    {
        _statusHistory.Add(new ApplicationStatusHistoryEntry(
            Guid.CreateVersion7(),
            status,
            occurredAtUtc,
            changedBy));
    }
}
