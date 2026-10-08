namespace ResearchAtlas.Domain.Entities;

public sealed class PersonalAddress
{
    internal PersonalAddress(
        Guid id,
        string addressLine,
        string postalCode,
        string locality,
        string administrativeArea,
        string countryCode)
    {
        Id = id;
        AddressLine = addressLine;
        PostalCode = postalCode;
        Locality = locality;
        AdministrativeArea = administrativeArea;
        CountryCode = countryCode;
    }

    public Guid Id { get; }

    public string AddressLine { get; }

    public string PostalCode { get; }

    public string Locality { get; }

    public string AdministrativeArea { get; }

    public string CountryCode { get; }
}
