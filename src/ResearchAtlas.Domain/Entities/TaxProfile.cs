namespace ResearchAtlas.Domain.Entities;

public sealed class TaxProfile
{
    internal TaxProfile(
        Guid id,
        string iban,
        Enums.TaxpayerStatus taxpayerStatus,
        Enums.TaxResidenceLocation taxResidenceLocation,
        string taxResidenceCountryCode,
        DateOnly? nonResidenceCertificateExpiresOn,
        string? foreignBankAccountNumber,
        string? swiftCode)
    {
        Id = id;
        Iban = iban;
        TaxpayerStatus = taxpayerStatus;
        TaxResidenceLocation = taxResidenceLocation;
        TaxResidenceCountryCode = taxResidenceCountryCode;
        NonResidenceCertificateExpiresOn = nonResidenceCertificateExpiresOn;
        ForeignBankAccountNumber = foreignBankAccountNumber;
        SwiftCode = swiftCode;
    }

    public Guid Id { get; }

    public string Iban { get; }

    public Enums.TaxpayerStatus TaxpayerStatus { get; }

    public Enums.TaxResidenceLocation TaxResidenceLocation { get; }

    public string TaxResidenceCountryCode { get; }

    public DateOnly? NonResidenceCertificateExpiresOn { get; }

    public string? ForeignBankAccountNumber { get; }

    public string? SwiftCode { get; }
}
