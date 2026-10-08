using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationAppealDecisionRequiresResolutionRule(ApplicationStatus status) : IBusinessRule
{
    public string RuleName => "Application.AppealDecisionRequiresResolution";

    public string ErrorMessage => "An appeal decision requires a positive or negative resolution.";

    public bool IsBroken() => status is not (ApplicationStatus.Positive or ApplicationStatus.Negative);

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
