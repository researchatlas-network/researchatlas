using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class CommitteeChairAndSecretaryMustBeDifferentPeopleRule(Guid chairId, Guid secretaryId) : IBusinessRule
{
    public string RuleName => "Committee.ChairAndSecretaryMustDiffer";

    public string ErrorMessage => "The chair and secretary must be different people.";

    public bool IsBroken() => chairId == secretaryId;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
