using ResearchAtlas.Domain.Aggregates;
using ResearchAtlas.Domain.Enums;
using ResearchAtlas.Domain.Factories;
using Xunit;

namespace ResearchAtlas.Domain.Tests;

public sealed class CurriculumVitaeFactoryTests
{
    [Fact]
    public void Create_AssignsOwnerOrcidAndKnowledgeAreas()
    {
        var person = CreatePerson();
        var firstKnowledgeAreaId = Guid.CreateVersion7();
        var secondKnowledgeAreaId = Guid.CreateVersion7();

        var curriculumVitae = CurriculumVitaeFactory.Create(
            Guid.CreateVersion7(),
            person,
            "0000-0002-1825-0097",
            [firstKnowledgeAreaId, secondKnowledgeAreaId]);

        Assert.Same(person, curriculumVitae.Person);
        Assert.Equal("0000-0002-1825-0097", curriculumVitae.Orcid);
        Assert.Equal([firstKnowledgeAreaId, secondKnowledgeAreaId], curriculumVitae.KnowledgeAreaIds);
    }

    [Fact]
    public void SetOrcid_RejectsAnInvalidOrcid()
    {
        var curriculumVitae = CurriculumVitaeFactory.Create(Guid.CreateVersion7(), CreatePerson(), null, []);

        var exception = Assert.Throws<ArgumentException>(() => curriculumVitae.SetOrcid("0000-0002-1825-0098"));

        Assert.Equal("orcid", exception.ParamName);
    }

    [Fact]
    public void AddPublication_StoresPublicationIdentifiers()
    {
        var curriculumVitae = CurriculumVitaeFactory.Create(Guid.CreateVersion7(), CreatePerson(), null, []);
        var publicationId = Guid.CreateVersion7();
        var reviewedBy = CreatePerson();
        var reviewedAtUtc = new DateTimeOffset(2026, 9, 18, 10, 0, 0, TimeSpan.Zero);

        curriculumVitae.AddPublication(
            publicationId,
            PublicationType.Article,
            2026,
            " 10.1000/example-doi ",
            "1234-5678",
            "8765-4321",
            "978-1-4028-9462-6",
            "978-1-4028-9463-3",
            "  Identifiers verified.  ",
            reviewedBy,
            reviewedAtUtc);

        var publication = Assert.Single(curriculumVitae.Publications);
        Assert.Equal(publicationId, publication.Id);
        Assert.Equal(PublicationType.Article, publication.Type);
        Assert.Equal(2026, publication.PublicationYear);
        Assert.Equal("10.1000/example-doi", publication.Doi);
        Assert.Equal("1234-5678", publication.Issn);
        Assert.Equal("8765-4321", publication.ElectronicIssn);
        Assert.Equal("978-1-4028-9462-6", publication.Isbn);
        Assert.Equal("978-1-4028-9463-3", publication.ElectronicIsbn);
        Assert.Equal("Identifiers verified.", publication.ReviewComments);
        Assert.Equal(reviewedBy, publication.ReviewedBy);
        Assert.Equal(reviewedAtUtc, publication.ReviewedAtUtc);
    }

    private static Person CreatePerson()
    {
        return PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            Sex.Female);
    }
}
