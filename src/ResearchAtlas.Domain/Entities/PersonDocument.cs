namespace ResearchAtlas.Domain.Entities;

public sealed class PersonDocument
{
    internal PersonDocument(
        Guid id,
        Enums.PersonDocumentType documentType,
        string storageKey,
        string fileName,
        string contentType,
        DateTimeOffset addedAtUtc)
    {
        Id = id;
        DocumentType = documentType;
        StorageKey = storageKey;
        FileName = fileName;
        ContentType = contentType;
        AddedAtUtc = addedAtUtc;
    }

    public Guid Id { get; }

    public Enums.PersonDocumentType DocumentType { get; }

    // This key is resolved by the configured storage provider, never stored document content.
    public string StorageKey { get; }

    public string FileName { get; }

    public string ContentType { get; }

    public DateTimeOffset AddedAtUtc { get; }
}
