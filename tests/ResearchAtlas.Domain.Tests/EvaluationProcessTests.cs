using ResearchAtlas.Domain.Enums;
using ResearchAtlas.Domain.Factories;
using Xunit;

namespace ResearchAtlas.Domain.Tests;

public sealed class EvaluationProcessTests
{
    [Fact]
    public void AddMember_ReplacingTheChairPreservesTheFormerChairInTheMembershipHistory()
    {
        var committee = CreateCommittee(CreatePerson());
        var formerChair = committee.Chair;
        var replacement = CreatePerson();

        committee.AddMember(
            Guid.CreateVersion7(),
            replacement,
            CommitteeMemberRole.Chair,
            DateTimeOffset.UtcNow,
            null);
        committee.MarkAppointmentDocumentNotified(committee.Chair.Id);
        committee.MarkCessationDocumentNotified(formerChair.Id);
        committee.MarkAppreciationCertificateNotified(formerChair.Id);

        Assert.Equal(replacement, committee.Chair.Person);
        Assert.True(committee.Chair.IsAppointmentDocumentNotified);
        Assert.False(formerChair.IsActive);
        Assert.Equal("Replaced by a newly appointed chair.", formerChair.CessationReason);
        Assert.True(formerChair.IsCessationDocumentNotified);
        Assert.True(formerChair.IsAppreciationCertificateNotified);
        Assert.Contains(formerChair, committee.Members);
        Assert.Equal(CommitteeMemberRole.Chair, formerChair.Role);
    }

    [Fact]
    public void CreateReevaluation_LinksItToAnAcceptedAppealForTheSameApplication()
    {
        var application = CreateApplication();
        var user = CreateUser();
        var committee = CreateCommittee(user.Person);
        var initialEvaluation = EvaluationProcessFactory.CreateInitial(Guid.CreateVersion7(), application, committee, user);
        var evaluator = CreatePerson();
        evaluator.SignEvaluatorCodeOfEthics(DateTimeOffset.UtcNow);
        evaluator.ApproveForEvaluatorCollaboration(DateTimeOffset.UtcNow);
        var evaluatorReportTemplateId = Guid.CreateVersion7();
        var finalReportTemplateId = Guid.CreateVersion7();
        initialEvaluation.AddEvaluatorReport(Guid.CreateVersion7(), evaluatorReportTemplateId, evaluator, false, "The proposal does not meet the required quality threshold.");
        initialEvaluation.SetFinalReport(Guid.CreateVersion7(), finalReportTemplateId, false, "The evaluation is negative.");
        initialEvaluation.Complete(EvaluationProcessOutcome.Negative, DateTimeOffset.UtcNow, user);
        var appealReview = EvaluationProcessFactory.CreateAppealReview(Guid.CreateVersion7(), application, committee, user);
        appealReview.SetFinalReport(Guid.CreateVersion7(), Guid.CreateVersion7(), true, "The appeal provides sufficient grounds for reevaluation.");
        appealReview.Complete(EvaluationProcessOutcome.AppealAccepted, DateTimeOffset.UtcNow, user);

        var reevaluation = EvaluationProcessFactory.CreateReevaluation(Guid.CreateVersion7(), appealReview, committee, user);

        Assert.Equal(application, reevaluation.Application);
        Assert.Equal(EvaluationProcessType.Reevaluation, reevaluation.Type);
        Assert.Equal(appealReview, reevaluation.PrecedingProcess);
        Assert.Equal(EvaluationProcessStatus.InProgress, reevaluation.Status);
        var evaluatorReport = Assert.Single(initialEvaluation.EvaluatorReports);
        Assert.False(evaluatorReport.IsPositive);
        Assert.Equal(evaluatorReportTemplateId, evaluatorReport.ReportTemplateId);
        Assert.Equal("The proposal does not meet the required quality threshold.", evaluatorReport.Motivation);
        Assert.NotNull(initialEvaluation.FinalReport);
        Assert.False(initialEvaluation.FinalReport.IsPositive);
        Assert.Equal(finalReportTemplateId, initialEvaluation.FinalReport.ReportTemplateId);
        Assert.Equal(committee.Chair.Person, initialEvaluation.FinalReport.CommitteeChair);
        Assert.Equal(committee.Secretary.Person, initialEvaluation.FinalReport.CommitteeSecretary);
    }

    [Fact]
    public void CreateReevaluation_RejectsAnAppealReviewThatWasNotAccepted()
    {
        var application = CreateApplication();
        var user = CreateUser();
        var committee = CreateCommittee(user.Person);
        var appealReview = EvaluationProcessFactory.CreateAppealReview(Guid.CreateVersion7(), application, committee, user);
        appealReview.SetFinalReport(Guid.CreateVersion7(), Guid.CreateVersion7(), false, "The appeal does not provide sufficient grounds.");
        appealReview.Complete(EvaluationProcessOutcome.AppealRejected, DateTimeOffset.UtcNow, user);

        var exception = Assert.Throws<ArgumentException>(() => EvaluationProcessFactory.CreateReevaluation(
            Guid.CreateVersion7(),
            appealReview,
            committee,
            user));

        Assert.Equal("acceptedAppealReview", exception.ParamName);
    }

    private static ResearchAtlas.Domain.Aggregates.Application CreateApplication()
    {
        var call = CallForApplicationsFactory.Create(
            Guid.CreateVersion7(),
            "Research funding 2026",
            CallType.ResearchFunding,
            new DateTimeOffset(2026, 6, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero));

        return ApplicationFactory.Create(Guid.CreateVersion7(), call, CreatePerson(), CreateUser());
    }

    private static ResearchAtlas.Domain.Aggregates.Person CreatePerson()
    {
        return PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            Sex.Female);
    }

    private static ResearchAtlas.Domain.Aggregates.User CreateUser()
    {
        return UserFactory.Create(Guid.CreateVersion7(), CreatePerson());
    }

    private static ResearchAtlas.Domain.Aggregates.Committee CreateCommittee(ResearchAtlas.Domain.Aggregates.Person chair)
    {
        var classification = KnowledgeClassificationFactory.Create(Guid.CreateVersion7(), "Research areas");
        classification.AddScope(Guid.CreateVersion7(), "SCI", "Sciences");

        return CommitteeFactory.Create(
            Guid.CreateVersion7(),
            "Sciences committee",
            classification.Scopes.Single(),
            chair,
            CreatePerson());
    }
}
