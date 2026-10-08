using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationEligibilityCanOnlyBeReviewedAfterSubmissionRule(Enums.ApplicationStatus status) : IBusinessRule
{
    public string RuleName => "Application.EligibilityReviewRequiresSubmission";

    public string ErrorMessage => "Only a submitted application can enter eligibility review.";

    public bool IsBroken() => status != Enums.ApplicationStatus.Submitted;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
