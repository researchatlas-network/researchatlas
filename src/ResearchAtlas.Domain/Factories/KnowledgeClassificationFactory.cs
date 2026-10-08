using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class KnowledgeClassificationFactory
{
    public static KnowledgeClassification Create(Guid id, string name)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new KnowledgeClassification(id, name.Trim());
    }
}
