using ResearchAtlas.Domain.Enums;
using ResearchAtlas.Domain.Factories;
using Xunit;

namespace ResearchAtlas.Domain.Tests;

public sealed class PersonFactoryTests
{
    [Fact]
    public void Create_AssignsSexAndOptionalNationality()
    {
        var nationality = NationalityFactory.Create(Guid.CreateVersion7(), "ES", "Spanish");

        var person = PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            Sex.Female,
            nationality);

        Assert.Equal(Sex.Female, person.Sex);
        Assert.Same(nationality, person.Nationality);
    }

    [Fact]
    public void Create_AssignsOptionalContactDetails()
    {
        var person = PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            Sex.Female,
            emailAddress: " ada@example.org ",
            mobilePhoneNumber: "+34600123456");

        Assert.Equal("ada@example.org", person.EmailAddress);
        Assert.Equal("+34600123456", person.MobilePhoneNumber);
    }

    [Fact]
    public void Create_RejectsAnUndefinedSex()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            (Sex)99));

        Assert.Equal("sex", exception.ParamName);
    }

    [Fact]
    public void AddDocument_StoresMetadataAndStorageKey()
    {
        var person = PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            Sex.Female);

        person.AddDocument(
            Guid.CreateVersion7(),
            PersonDocumentType.CurriculumVitae,
            "persons/ada-lovelace/cv.pdf",
            "ada-lovelace-cv.pdf",
            "application/pdf");

        var document = Assert.Single(person.Documents);
        Assert.Equal(PersonDocumentType.CurriculumVitae, document.DocumentType);
        Assert.Equal("persons/ada-lovelace/cv.pdf", document.StorageKey);
        Assert.Equal("ada-lovelace-cv.pdf", document.FileName);
        Assert.Equal("application/pdf", document.ContentType);
    }

    [Fact]
    public void AddEvaluatorAssessment_PreservesTheAssessmentHistory()
    {
        var person = CreatePerson();
        var assessedBy = UserFactory.Create(Guid.CreateVersion7(), CreatePerson());
        var firstAssessmentId = Guid.CreateVersion7();
        var secondAssessmentId = Guid.CreateVersion7();

        person.AddEvaluatorAssessment(firstAssessmentId, true, "  Delivers thorough reports.  ", assessedBy);
        person.AddEvaluatorAssessment(secondAssessmentId, false, "Missed the evaluation deadline.", assessedBy);

        var assessments = person.EvaluatorAssessments.ToList();
        Assert.Equal(2, assessments.Count);
        Assert.Equal(firstAssessmentId, assessments[0].Id);
        Assert.True(assessments[0].ShouldRepeatAsEvaluator);
        Assert.Equal("Delivers thorough reports.", assessments[0].Comments);
        Assert.Same(assessedBy, assessments[0].AssessedBy);
        Assert.True(assessments[0].AssessedAtUtc <= DateTimeOffset.UtcNow);
        Assert.Equal(secondAssessmentId, assessments[1].Id);
        Assert.False(assessments[1].ShouldRepeatAsEvaluator);
    }

    [Fact]
    public void SignEvaluatorCodeOfEthics_RecordsTheSignatureDate()
    {
        var person = CreatePerson();
        var signedAtUtc = new DateTimeOffset(2026, 9, 18, 10, 0, 0, TimeSpan.Zero);

        person.SignEvaluatorCodeOfEthics(signedAtUtc);

        Assert.True(person.HasSignedEvaluatorCodeOfEthics);
        Assert.Equal(signedAtUtc, person.EvaluatorCodeOfEthicsSignedAtUtc);
    }

    [Fact]
    public void ApproveForEvaluatorCollaboration_RecordsTheApprovalDateAfterTheCodeOfEthicsIsSigned()
    {
        var person = CreatePerson();
        person.SignEvaluatorCodeOfEthics(new DateTimeOffset(2026, 9, 18, 10, 0, 0, TimeSpan.Zero));
        var approvedAtUtc = new DateTimeOffset(2026, 9, 18, 11, 0, 0, TimeSpan.Zero);

        person.ApproveForEvaluatorCollaboration(approvedAtUtc);

        Assert.True(person.IsApprovedForEvaluatorCollaboration);
        Assert.Equal(approvedAtUtc, person.ApprovedForEvaluatorCollaborationAtUtc);
    }

    [Fact]
    public void SetTaxProfile_StoresNonResidenceCertificateExpirationDate()
    {
        var person = CreatePerson();
        var expirationDate = new DateOnly(2027, 6, 30);

        person.SetTaxProfile(
            Guid.CreateVersion7(),
            "ES91 2100 0418 4502 0005 1332",
            TaxpayerStatus.Employee,
            TaxResidenceLocation.OutsideEuropeanUnion,
            "US",
            expirationDate);

        Assert.NotNull(person.TaxProfile);
        Assert.Equal(TaxResidenceLocation.OutsideEuropeanUnion, person.TaxProfile.TaxResidenceLocation);
        Assert.Equal(expirationDate, person.TaxProfile.NonResidenceCertificateExpiresOn);
    }

    [Fact]
    public void SetTaxProfile_RequiresCertificateExpirationDateForNonResident()
    {
        var person = CreatePerson();

        var exception = Assert.Throws<ArgumentException>(() => person.SetTaxProfile(
            Guid.CreateVersion7(),
            "ES91 2100 0418 4502 0005 1332",
            TaxpayerStatus.Employee,
            TaxResidenceLocation.EuropeanUnion,
            "FR"));

        Assert.Equal("nonResidenceCertificateExpiresOn", exception.ParamName);
    }

    [Fact]
    public void SetTaxProfile_StoresNormalizedFiscalInformation()
    {
        var person = CreatePerson();

        person.SetTaxProfile(
            Guid.CreateVersion7(),
            "ES91 2100 0418 4502 0005 1332",
            TaxpayerStatus.SelfEmployed,
            TaxResidenceLocation.Spain,
            "es");

        Assert.NotNull(person.TaxProfile);
        Assert.Equal("ES9121000418450200051332", person.TaxProfile.Iban);
        Assert.Equal(TaxpayerStatus.SelfEmployed, person.TaxProfile.TaxpayerStatus);
        Assert.Equal(TaxResidenceLocation.Spain, person.TaxProfile.TaxResidenceLocation);
        Assert.Equal("ES", person.TaxProfile.TaxResidenceCountryCode);
    }

    [Fact]
    public void SetTaxProfile_StoresForeignBankAccountAndSwiftCode()
    {
        var person = CreatePerson();

        person.SetTaxProfile(
            Guid.CreateVersion7(),
            "ES91 2100 0418 4502 0005 1332",
            TaxpayerStatus.Employee,
            TaxResidenceLocation.EuropeanUnion,
            "FR",
            new DateOnly(2027, 6, 30),
            "1234567890",
            "deut de ff");

        Assert.NotNull(person.TaxProfile);
        Assert.Equal("1234567890", person.TaxProfile.ForeignBankAccountNumber);
        Assert.Equal("DEUTDEFF", person.TaxProfile.SwiftCode);
    }

    [Fact]
    public void SetTaxProfile_RequiresSwiftCodeForForeignBankAccount()
    {
        var person = CreatePerson();

        var exception = Assert.Throws<ArgumentException>(() => person.SetTaxProfile(
            Guid.CreateVersion7(),
            "ES91 2100 0418 4502 0005 1332",
            TaxpayerStatus.Employee,
            TaxResidenceLocation.EuropeanUnion,
            "FR",
            new DateOnly(2027, 6, 30),
            "1234567890"));

        Assert.Equal("foreignBankAccountNumber", exception.ParamName);
    }

    [Fact]
    public void SetPersonalAddress_StoresOneNormalizedAddress()
    {
        var person = CreatePerson();

        person.SetPersonalAddress(
            Guid.CreateVersion7(),
            "  10 Analytical Engine Street  ",
            " SW1A 1AA ",
            " London ",
            " Greater London ",
            " gb ");

        Assert.NotNull(person.PersonalAddress);
        Assert.Equal("10 Analytical Engine Street", person.PersonalAddress.AddressLine);
        Assert.Equal("SW1A 1AA", person.PersonalAddress.PostalCode);
        Assert.Equal("London", person.PersonalAddress.Locality);
        Assert.Equal("Greater London", person.PersonalAddress.AdministrativeArea);
        Assert.Equal("GB", person.PersonalAddress.CountryCode);
    }

    private static ResearchAtlas.Domain.Aggregates.Person CreatePerson()
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
