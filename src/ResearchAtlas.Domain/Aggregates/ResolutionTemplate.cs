namespace ResearchAtlas.Domain.Aggregates;

public sealed class ResolutionTemplate
{
    internal ResolutionTemplate(Guid id, string name, string storageKey, string fileName, string contentType)
    {
        Id = id;
        Name = name;
        StorageKey = storageKey;
        FileName = fileName;
        ContentType = contentType;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string StorageKey { get; }

    public string FileName { get; }

    public string ContentType { get; }
}
