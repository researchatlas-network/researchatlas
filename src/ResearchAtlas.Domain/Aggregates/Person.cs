namespace ResearchAtlas.Domain.Aggregates;

public sealed class Person
{
    private static readonly HashSet<string> EuropeanUnionCountryCodes =
    [
        "AT", "BE", "BG", "HR", "CY", "CZ", "DK", "EE", "FI", "FR", "DE", "GR", "HU", "IE",
        "IT", "LV", "LT", "LU", "MT", "NL", "PL", "PT", "RO", "SK", "SI", "ES", "SE"
    ];

    private readonly List<ResearchAtlas.Domain.Entities.PersonNote> _notes = [];
    private readonly List<ResearchAtlas.Domain.Entities.PersonDocument> _documents = [];
    private readonly List<ResearchAtlas.Domain.Entities.EvaluatorAssessment> _evaluatorAssessments = [];

    internal Person(
        Guid id,
        string givenName,
        string firstSurname,
        string? secondSurname,
        DateOnly birthDate,
        string preferredCulture,
        Enums.IdentityDocumentType identityDocumentType,
        string documentNumber,
        Enums.Sex sex,
        Nationality? nationality,
        string? emailAddress,
        string? mobilePhoneNumber)
    {
        Id = id;
        GivenName = givenName;
        FirstSurname = firstSurname;
        SecondSurname = secondSurname;
        BirthDate = birthDate;
        PreferredCulture = preferredCulture;
        IdentityDocumentType = identityDocumentType;
        DocumentNumber = documentNumber;
        Sex = sex;
        Nationality = nationality;
        EmailAddress = emailAddress;
        MobilePhoneNumber = mobilePhoneNumber;
    }

    public Guid Id { get; }

    public string GivenName { get; }

    public string FirstSurname { get; }

    public string? SecondSurname { get; }

    public string FullName => SecondSurname is null
        ? $"{GivenName} {FirstSurname}"
        : $"{GivenName} {FirstSurname} {SecondSurname}";

    public DateOnly BirthDate { get; }

    public int Age
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - BirthDate.Year;

            if (today.Month < BirthDate.Month ||
                (today.Month == BirthDate.Month && today.Day < BirthDate.Day))
            {
                age--;
            }

            return age;
        }
    }

    public string PreferredCulture { get; }

    public Enums.IdentityDocumentType IdentityDocumentType { get; }

    public string DocumentNumber { get; }

    public Enums.Sex Sex { get; }

    public Nationality? Nationality { get; }

    public string? EmailAddress { get; }

    public string? MobilePhoneNumber { get; }

    public DateTimeOffset? EvaluatorCodeOfEthicsSignedAtUtc { get; private set; }

    public bool HasSignedEvaluatorCodeOfEthics => EvaluatorCodeOfEthicsSignedAtUtc.HasValue;

    public DateTimeOffset? ApprovedForEvaluatorCollaborationAtUtc { get; private set; }

    public bool IsApprovedForEvaluatorCollaboration => ApprovedForEvaluatorCollaborationAtUtc.HasValue;

    public IReadOnlyCollection<ResearchAtlas.Domain.Entities.PersonNote> Notes => _notes.AsReadOnly();

    public IReadOnlyCollection<ResearchAtlas.Domain.Entities.PersonDocument> Documents => _documents.AsReadOnly();

    public IReadOnlyCollection<ResearchAtlas.Domain.Entities.EvaluatorAssessment> EvaluatorAssessments => _evaluatorAssessments.AsReadOnly();

    public ResearchAtlas.Domain.Entities.TaxProfile? TaxProfile { get; private set; }

    public ResearchAtlas.Domain.Entities.PersonalAddress? PersonalAddress { get; private set; }

    public void AddNote(Guid id, string content, bool isCritical, User addedBy)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentNullException.ThrowIfNull(addedBy);

        _notes.Add(new ResearchAtlas.Domain.Entities.PersonNote(
            id,
            content.Trim(),
            isCritical,
            addedBy,
            DateTimeOffset.UtcNow));
    }

    public void AddDocument(
        Guid id,
        Enums.PersonDocumentType documentType,
        string storageKey,
        string fileName,
        string contentType)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);

        if (!Enum.IsDefined(documentType))
        {
            throw new ArgumentOutOfRangeException(nameof(documentType));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);

        _documents.Add(new ResearchAtlas.Domain.Entities.PersonDocument(
            id,
            documentType,
            storageKey.Trim(),
            fileName.Trim(),
            contentType.Trim(),
            DateTimeOffset.UtcNow));
    }

    public void AddEvaluatorAssessment(Guid id, bool shouldRepeatAsEvaluator, string comments, User assessedBy)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(comments);
        ArgumentNullException.ThrowIfNull(assessedBy);

        _evaluatorAssessments.Add(new ResearchAtlas.Domain.Entities.EvaluatorAssessment(
            id,
            shouldRepeatAsEvaluator,
            comments.Trim(),
            assessedBy,
            DateTimeOffset.UtcNow));
    }

    public void SignEvaluatorCodeOfEthics(DateTimeOffset signedAtUtc)
    {
        if (HasSignedEvaluatorCodeOfEthics)
        {
            throw new InvalidOperationException("The evaluator code of ethics has already been signed.");
        }

        EvaluatorCodeOfEthicsSignedAtUtc = signedAtUtc;
    }

    public void ApproveForEvaluatorCollaboration(DateTimeOffset approvedAtUtc)
    {
        if (!HasSignedEvaluatorCodeOfEthics)
        {
            throw new InvalidOperationException("The evaluator code of ethics must be signed before evaluator collaboration can be approved.");
        }

        if (IsApprovedForEvaluatorCollaboration)
        {
            throw new InvalidOperationException("Evaluator collaboration has already been approved.");
        }

        ApprovedForEvaluatorCollaborationAtUtc = approvedAtUtc;
    }

    public void SetTaxProfile(
        Guid id,
        string iban,
        Enums.TaxpayerStatus taxpayerStatus,
        Enums.TaxResidenceLocation taxResidenceLocation,
        string taxResidenceCountryCode,
        DateOnly? nonResidenceCertificateExpiresOn = null,
        string? foreignBankAccountNumber = null,
        string? swiftCode = null)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(iban);

        if (!Enum.IsDefined(taxpayerStatus))
        {
            throw new ArgumentOutOfRangeException(nameof(taxpayerStatus));
        }

        if (!Enum.IsDefined(taxResidenceLocation))
        {
            throw new ArgumentOutOfRangeException(nameof(taxResidenceLocation));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(taxResidenceCountryCode);

        var normalizedIban = string.Concat(iban.Where(char.IsLetterOrDigit)).ToUpperInvariant();

        if (!IsValidIban(normalizedIban))
        {
            throw new ArgumentException("The IBAN must be a valid identifier.", nameof(iban));
        }

        var normalizedTaxResidenceCountryCode = taxResidenceCountryCode.Trim().ToUpperInvariant();

        if (normalizedTaxResidenceCountryCode.Length != 2 || !normalizedTaxResidenceCountryCode.All(char.IsLetter))
        {
            throw new ArgumentException("The tax residence country code must use ISO 3166-1 alpha-2 format.", nameof(taxResidenceCountryCode));
        }

        if (taxResidenceLocation == Enums.TaxResidenceLocation.Spain && normalizedTaxResidenceCountryCode != "ES")
        {
            throw new ArgumentException("A taxpayer resident in Spain must use ES as the tax residence country code.", nameof(taxResidenceCountryCode));
        }

        if (taxResidenceLocation == Enums.TaxResidenceLocation.EuropeanUnion && !EuropeanUnionCountryCodes.Contains(normalizedTaxResidenceCountryCode))
        {
            throw new ArgumentException("A taxpayer resident in the European Union must use an EU country code.", nameof(taxResidenceCountryCode));
        }

        if (taxResidenceLocation == Enums.TaxResidenceLocation.OutsideEuropeanUnion && EuropeanUnionCountryCodes.Contains(normalizedTaxResidenceCountryCode))
        {
            throw new ArgumentException("A taxpayer resident outside the European Union cannot use an EU country code.", nameof(taxResidenceCountryCode));
        }

        if (taxResidenceLocation != Enums.TaxResidenceLocation.Spain && !nonResidenceCertificateExpiresOn.HasValue)
        {
            throw new ArgumentException("A taxpayer resident outside Spain must have a non-residence certificate expiration date.", nameof(nonResidenceCertificateExpiresOn));
        }

        if (taxResidenceLocation == Enums.TaxResidenceLocation.Spain && nonResidenceCertificateExpiresOn.HasValue)
        {
            throw new ArgumentException("A taxpayer resident in Spain cannot have a non-residence certificate expiration date.", nameof(nonResidenceCertificateExpiresOn));
        }

        var normalizedForeignBankAccountNumber = string.IsNullOrWhiteSpace(foreignBankAccountNumber) ? null : foreignBankAccountNumber.Trim();
        var normalizedSwiftCode = string.IsNullOrWhiteSpace(swiftCode) ? null : string.Concat(swiftCode.Where(char.IsLetterOrDigit)).ToUpperInvariant();

        if ((normalizedForeignBankAccountNumber is null) != (normalizedSwiftCode is null))
        {
            throw new ArgumentException("A foreign bank account number and a SWIFT code must be provided together.", nameof(foreignBankAccountNumber));
        }

        if (normalizedSwiftCode is not null && !IsValidSwiftCode(normalizedSwiftCode))
        {
            throw new ArgumentException("The SWIFT code must be a valid BIC.", nameof(swiftCode));
        }

        TaxProfile = new ResearchAtlas.Domain.Entities.TaxProfile(
            id,
            normalizedIban,
            taxpayerStatus,
            taxResidenceLocation,
            normalizedTaxResidenceCountryCode,
            nonResidenceCertificateExpiresOn,
            normalizedForeignBankAccountNumber,
            normalizedSwiftCode);
    }

    public void SetPersonalAddress(
        Guid id,
        string addressLine,
        string postalCode,
        string locality,
        string administrativeArea,
        string countryCode)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(locality);
        ArgumentException.ThrowIfNullOrWhiteSpace(administrativeArea);
        ArgumentException.ThrowIfNullOrWhiteSpace(countryCode);

        var normalizedCountryCode = countryCode.Trim().ToUpperInvariant();

        if (normalizedCountryCode.Length != 2 || !normalizedCountryCode.All(char.IsLetter))
        {
            throw new ArgumentException("The country code must use ISO 3166-1 alpha-2 format.", nameof(countryCode));
        }

        PersonalAddress = new ResearchAtlas.Domain.Entities.PersonalAddress(
            id,
            addressLine.Trim(),
            postalCode.Trim(),
            locality.Trim(),
            administrativeArea.Trim(),
            normalizedCountryCode);
    }

    private static bool IsValidIban(string iban)
    {
        if (iban.Length is < 15 or > 34 || !iban[..2].All(char.IsLetter) || !iban[2..4].All(char.IsDigit) || !iban.All(char.IsLetterOrDigit))
        {
            return false;
        }

        var rearrangedIban = string.Concat(iban[4..], iban[..4]);
        var remainder = 0;

        foreach (var character in rearrangedIban)
        {
            var value = char.IsLetter(character) ? character - 'A' + 10 : character - '0';
            remainder = (remainder * (value >= 10 ? 100 : 10) + value) % 97;
        }

        return remainder == 1;
    }

    private static bool IsValidSwiftCode(string swiftCode)
    {
        return swiftCode.Length is 8 or 11 &&
            swiftCode[..4].All(char.IsLetter) &&
            swiftCode[4..6].All(char.IsLetter) &&
            swiftCode[6..8].All(char.IsLetterOrDigit) &&
            (swiftCode.Length == 8 || swiftCode[8..].All(char.IsLetterOrDigit));
    }
}
