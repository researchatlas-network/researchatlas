using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Enums;
using ResearchAtlas.Domain.Factories;
using Xunit;

namespace ResearchAtlas.Domain.Tests;

public sealed class CallForApplicationsTests
{
    [Fact]
    public void Create_OpensTheCallForApplications()
    {
        var call = CreateCall();

        Assert.True(call.IsOpenForApplications);
    }

    [Fact]
    public void Create_SetsTheCallType()
    {
        var call = CreateCall();

        Assert.Equal(CallType.ResearchActivityAssessment, call.Type);
    }

    [Fact]
    public void Submit_RejectsApplicationsWhenTheCallIsClosed()
    {
        var call = CreateCall();
        var user = CreateUser();
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), call, CreatePerson(), user);
        call.CloseApplications();

        var exception = Assert.Throws<BusinessRuleException>(() => application.Submit(new DateTimeOffset(2026, 6, 15, 0, 0, 0, TimeSpan.Zero), user));

        Assert.Contains(exception.BrokenRules, rule => rule.RuleName == "Application.SubmissionRequiresOpenCall");
    }

    [Fact]
    public void Approve_RecordsTheUserAndDateWhenTheCallHasAConfiguredTemplate()
    {
        var call = CreateCall();
        var template = ResolutionTemplateFactory.Create(
            Guid.CreateVersion7(),
            "Positive resolution",
            "resolution-templates/positive.html",
            "positive.html",
            "text/html");
        var user = CreateUser();
        call.ConfigureResolutionTemplate(ApplicationStatus.Positive, template);
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), call, CreatePerson(), user);
        MoveToUnderReview(application, user);
        var occurredAtUtc = new DateTimeOffset(2026, 6, 15, 0, 0, 0, TimeSpan.Zero);

        application.Approve(occurredAtUtc, user);

        Assert.Equal(ApplicationStatus.Positive, application.Status);
        var statusChange = application.StatusHistory.Last();
        Assert.Equal(ApplicationStatus.Positive, statusChange.Status);
        Assert.Equal(occurredAtUtc, statusChange.OccurredAtUtc);
        Assert.Equal(user, statusChange.ChangedBy);
    }

    [Fact]
    public void Approve_RejectsApplicationsThatAreNotUnderReview()
    {
        var call = CreateCall();
        var user = CreateUser();
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), call, CreatePerson(), user);

        var exception = Assert.Throws<BusinessRuleException>(() => application.Approve(DateTimeOffset.UtcNow, user));

        Assert.Contains(exception.BrokenRules, rule => rule.RuleName == "Application.ResolutionRequiresReview");
    }

    [Fact]
    public void Reject_RejectsApplicationsThatAreNotUnderReview()
    {
        var user = CreateUser();
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), CreateCall(), CreatePerson(), user);

        var exception = Assert.Throws<BusinessRuleException>(() => application.Reject(DateTimeOffset.UtcNow, user));

        Assert.Contains(exception.BrokenRules, rule => rule.RuleName == "Application.ResolutionRequiresReview");
    }

    [Fact]
    public void AcceptAppeal_ChangesAResolvedApplicationToAppealAccepted()
    {
        var call = CreateCall();
        var user = CreateUser();
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), call, CreatePerson(), user);
        MoveToUnderReview(application, user);
        ConfigureResolutionTemplates(call);
        application.Approve(DateTimeOffset.UtcNow, user);

        application.AcceptAppeal(DateTimeOffset.UtcNow, user);

        Assert.Equal(ApplicationStatus.AppealAccepted, application.Status);
    }

    [Fact]
    public void RejectAppeal_RejectsApplicationsWithoutAResolution()
    {
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), CreateCall(), CreatePerson(), CreateUser());

        var exception = Assert.Throws<BusinessRuleException>(() => application.RejectAppeal(DateTimeOffset.UtcNow, CreateUser()));

        Assert.Contains(exception.BrokenRules, rule => rule.RuleName == "Application.AppealDecisionRequiresResolution");
    }

    [Fact]
    public void StartReview_RecordsTheReviewStatus()
    {
        var call = CreateCall();
        var user = CreateUser();
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), call, CreatePerson(), user);
        application.Submit(new DateTimeOffset(2026, 6, 15, 0, 0, 0, TimeSpan.Zero), user);
        application.StartEligibilityReview(new DateTimeOffset(2026, 6, 16, 0, 0, 0, TimeSpan.Zero), user);
        application.ConfirmEligibility(new DateTimeOffset(2026, 6, 17, 0, 0, 0, TimeSpan.Zero), user);
        var startedAtUtc = new DateTimeOffset(2026, 6, 18, 0, 0, 0, TimeSpan.Zero);

        application.StartReview(startedAtUtc, user);

        Assert.Equal(ApplicationStatus.UnderReview, application.Status);
        var statusChange = Assert.Single(application.StatusHistory, change => change.Status == ApplicationStatus.UnderReview);
        Assert.Equal(startedAtUtc, statusChange.OccurredAtUtc);
        Assert.Equal(user, statusChange.ChangedBy);
    }

    [Fact]
    public void AssignReviewer_SetsThePersonResponsibleForReviewingTheApplication()
    {
        var application = ApplicationFactory.Create(Guid.CreateVersion7(), CreateCall(), CreatePerson(), CreateUser());
        var reviewer = CreatePerson();

        application.AssignReviewer(reviewer);

        Assert.Equal(reviewer, application.Reviewer);
    }

    private static ResearchAtlas.Domain.Aggregates.CallForApplications CreateCall()
    {
        return CallForApplicationsFactory.Create(
            Guid.CreateVersion7(),
            "Research activity assessment 2026",
            CallType.ResearchActivityAssessment,
            new DateTimeOffset(2026, 6, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero));
    }

    private static void ConfigureResolutionTemplates(ResearchAtlas.Domain.Aggregates.CallForApplications call)
    {
        foreach (var status in new[] { ApplicationStatus.Positive, ApplicationStatus.AppealAccepted })
        {
            call.ConfigureResolutionTemplate(
                status,
                ResolutionTemplateFactory.Create(
                    Guid.CreateVersion7(),
                    $"{status} resolution",
                    $"resolution-templates/{status}.html",
                    $"{status}.html",
                    "text/html"));
        }
    }

    private static void MoveToUnderReview(ResearchAtlas.Domain.Aggregates.Application application, ResearchAtlas.Domain.Aggregates.User user)
    {
        application.Submit(new DateTimeOffset(2026, 6, 15, 0, 0, 0, TimeSpan.Zero), user);
        application.StartEligibilityReview(new DateTimeOffset(2026, 6, 16, 0, 0, 0, TimeSpan.Zero), user);
        application.ConfirmEligibility(new DateTimeOffset(2026, 6, 17, 0, 0, 0, TimeSpan.Zero), user);
        application.StartReview(new DateTimeOffset(2026, 6, 18, 0, 0, 0, TimeSpan.Zero), user);
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
}
