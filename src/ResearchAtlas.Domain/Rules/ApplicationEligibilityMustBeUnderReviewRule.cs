using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationEligibilityMustBeUnderReviewRule(Enums.ApplicationStatus status) : IBusinessRule
{
    public string RuleName => "Application.EligibilityDecisionRequiresReview";

    public string ErrorMessage => "An eligibility decision requires an application under eligibility review.";

    public bool IsBroken() => status != Enums.ApplicationStatus.UnderEligibilityReview;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
