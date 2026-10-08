using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class CurriculumVitaeFactory
{
    public static CurriculumVitae Create(Guid id, Person person, string? orcid, IEnumerable<Guid> knowledgeAreaIds)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(person);
        ArgumentNullException.ThrowIfNull(knowledgeAreaIds);

        var curriculumVitae = new CurriculumVitae(id, person, null, []);
        curriculumVitae.SetOrcid(orcid);
        curriculumVitae.SetKnowledgeAreas(knowledgeAreaIds);

        return curriculumVitae;
    }
}
