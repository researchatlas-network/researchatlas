using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Aggregates;
using ResearchAtlas.Domain.Entities;
using ResearchAtlas.Domain.Rules;

namespace ResearchAtlas.Domain.Factories;

public static class CommitteeFactory
{
    public static Committee Create(Guid id, string name, KnowledgeScope knowledgeScope, Person chair, Person secretary)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(knowledgeScope);
        ArgumentNullException.ThrowIfNull(chair);
        ArgumentNullException.ThrowIfNull(secretary);

        BusinessRuleValidator.Validate(new CommitteeChairAndSecretaryMustBeDifferentPeopleRule(chair.Id, secretary.Id));

        return new Committee(id, name.Trim(), knowledgeScope, chair, secretary);
    }
}
