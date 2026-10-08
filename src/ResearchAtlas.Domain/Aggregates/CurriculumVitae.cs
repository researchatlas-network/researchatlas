using ResearchAtlas.Domain.Entities;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class CurriculumVitae
{
    private readonly List<Guid> _knowledgeAreaIds = [];
    private readonly List<Publication> _publications = [];

    internal CurriculumVitae(Guid id, Person person, string? orcid, IEnumerable<Guid> knowledgeAreaIds)
    {
        Id = id;
        Person = person;
        Orcid = orcid;
        _knowledgeAreaIds.AddRange(knowledgeAreaIds);
    }

    public Guid Id { get; }

    public Person Person { get; }

    public string? Orcid { get; private set; }

    public IReadOnlyCollection<Guid> KnowledgeAreaIds => _knowledgeAreaIds.AsReadOnly();

    public IReadOnlyCollection<Publication> Publications => _publications.AsReadOnly();

    public void AddPublication(
        Guid id,
        Enums.PublicationType type,
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
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfLessThan(publicationYear, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(reviewComments);
        ArgumentNullException.ThrowIfNull(reviewedBy);

        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type));
        }

        var normalizedDoi = NormalizeIdentifier(doi);
        var normalizedIssn = NormalizeIdentifier(issn);
        var normalizedElectronicIssn = NormalizeIdentifier(electronicIssn);
        var normalizedIsbn = NormalizeIdentifier(isbn);
        var normalizedElectronicIsbn = NormalizeIdentifier(electronicIsbn);

        if (normalizedDoi is null && normalizedIssn is null && normalizedElectronicIssn is null && normalizedIsbn is null && normalizedElectronicIsbn is null)
        {
            throw new ArgumentException("A publication must have at least one identifier.");
        }

        _publications.Add(new Publication(
            id,
            type,
            publicationYear,
            normalizedDoi,
            normalizedIssn,
            normalizedElectronicIssn,
            normalizedIsbn,
            normalizedElectronicIsbn,
            reviewComments.Trim(),
            reviewedBy,
            reviewedAtUtc));
    }

    public void SetOrcid(string? orcid)
    {
        var normalizedOrcid = string.IsNullOrWhiteSpace(orcid) ? null : orcid.Trim().ToUpperInvariant();

        if (normalizedOrcid is not null && !IsValidOrcid(normalizedOrcid))
        {
            throw new ArgumentException("The ORCID must be a valid identifier.", nameof(orcid));
        }

        Orcid = normalizedOrcid;
    }

    public void SetKnowledgeAreas(IEnumerable<Guid> knowledgeAreaIds)
    {
        ArgumentNullException.ThrowIfNull(knowledgeAreaIds);

        var areaIds = knowledgeAreaIds.ToArray();

        if (areaIds.Any(areaId => areaId == Guid.Empty))
        {
            throw new ArgumentOutOfRangeException(nameof(knowledgeAreaIds));
        }

        if (areaIds.Distinct().Count() != areaIds.Length)
        {
            throw new ArgumentException("Knowledge areas must be unique.", nameof(knowledgeAreaIds));
        }

        _knowledgeAreaIds.Clear();
        _knowledgeAreaIds.AddRange(areaIds);
    }

    private static bool IsValidOrcid(string orcid)
    {
        var normalized = orcid.Replace("-", string.Empty, StringComparison.Ordinal);

        if (normalized.Length != 16 || !normalized[..15].All(char.IsDigit) || (!char.IsDigit(normalized[15]) && normalized[15] != 'X'))
        {
            return false;
        }

        var total = 0;

        foreach (var character in normalized[..15])
        {
            total = (total + character - '0') * 2;
        }

        var remainder = total % 11;
        var checkDigit = (12 - remainder) % 11;
        var expectedCheckCharacter = checkDigit == 10 ? 'X' : (char)('0' + checkDigit);

        return normalized[15] == expectedCheckCharacter;
    }

    private static string? NormalizeIdentifier(string? identifier)
    {
        return string.IsNullOrWhiteSpace(identifier) ? null : identifier.Trim();
    }
}
