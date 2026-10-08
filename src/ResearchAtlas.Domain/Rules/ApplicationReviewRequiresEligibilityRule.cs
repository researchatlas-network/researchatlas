using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationReviewRequiresEligibilityRule(ApplicationStatus status) : IBusinessRule
{
    public string RuleName => "Application.ReviewRequiresEligibility";

    public string ErrorMessage => "Only an eligible application can enter review.";

    public bool IsBroken() => status != ApplicationStatus.Eligible;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
