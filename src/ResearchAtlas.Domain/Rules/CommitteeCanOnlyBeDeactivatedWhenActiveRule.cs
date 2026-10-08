using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class CommitteeCanOnlyBeDeactivatedWhenActiveRule(bool isActive) : IBusinessRule
{
    public string RuleName => "Committee.DeactivationRequiresActiveCommittee";

    public string ErrorMessage => "Only an active committee can be deactivated.";

    public bool IsBroken() => !isActive;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
