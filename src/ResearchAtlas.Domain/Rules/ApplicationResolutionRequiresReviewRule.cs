using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationResolutionRequiresReviewRule(ApplicationStatus status) : IBusinessRule
{
    public string RuleName => "Application.ResolutionRequiresReview";

    public string ErrorMessage => "A resolution requires an application under review.";

    public bool IsBroken() => status != ApplicationStatus.UnderReview;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
