using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Aggregates;
using ResearchAtlas.Domain.Entities;

namespace ResearchAtlas.Domain.Rules;

public sealed class CommitteeMemberMustNotAlreadyBelongRule(
    Person person,
    IEnumerable<CommitteeMember> members) : IBusinessRule
{
    public string RuleName => "Committee.MemberMustBeUnique";

    public string ErrorMessage => "A person can only have one active membership in a committee.";

    public bool IsBroken() => members.Any(member => member.Person.Id == person.Id && member.IsActive);

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
