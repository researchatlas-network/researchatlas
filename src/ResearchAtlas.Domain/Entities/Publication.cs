using ResearchAtlas.Domain.Aggregates;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Entities;

public sealed class Publication
{
    internal Publication(
        Guid id,
        PublicationType type,
        int publicationYear,
        string? doi,
        string? issn,
        string? electronicIssn,
        string? isbn,
        string? electronicIsbn,
        string reviewComments,
        Person reviewedBy,
        DateTimeOffset reviewedAtUtc)
    {
        Id = id;
        Type = type;
        PublicationYear = publicationYear;
        Doi = doi;
        Issn = issn;
        ElectronicIssn = electronicIssn;
        Isbn = isbn;
        ElectronicIsbn = electronicIsbn;
        ReviewComments = reviewComments;
        ReviewedBy = reviewedBy;
        ReviewedAtUtc = reviewedAtUtc;
    }

    public Guid Id { get; }

    public PublicationType Type { get; }

    public int PublicationYear { get; }

    public string? Doi { get; }

    public string? Issn { get; }

    public string? ElectronicIssn { get; }

    public string? Isbn { get; }

    public string? ElectronicIsbn { get; }

    public string ReviewComments { get; }

    public Person ReviewedBy { get; }

    public DateTimeOffset ReviewedAtUtc { get; }
}
