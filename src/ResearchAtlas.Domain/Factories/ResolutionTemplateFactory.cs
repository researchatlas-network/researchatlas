using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class ResolutionTemplateFactory
{
    public static ResolutionTemplate Create(Guid id, string name, string storageKey, string fileName, string contentType)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);

        return new ResolutionTemplate(id, name.Trim(), storageKey.Trim(), fileName.Trim(), contentType.Trim());
    }
}
