using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class PersonFactory
{
    public static Person Create(
        Guid id,
        string givenName,
        string firstSurname,
        string? secondSurname,
        DateOnly birthDate,
        string preferredCulture,
        Enums.IdentityDocumentType identityDocumentType,
        string documentNumber,
        Enums.Sex sex,
        Nationality? nationality = null,
        string? emailAddress = null,
        string? mobilePhoneNumber = null)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(givenName);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstSurname);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(birthDate, DateOnly.FromDateTime(DateTime.UtcNow));

        ArgumentException.ThrowIfNullOrWhiteSpace(preferredCulture);

        var normalizedPreferredCulture = preferredCulture.Trim();

        if (!Enum.IsDefined(identityDocumentType))
        {
            throw new ArgumentOutOfRangeException(nameof(identityDocumentType));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(documentNumber);

        if (!Enum.IsDefined(sex))
        {
            throw new ArgumentOutOfRangeException(nameof(sex));
        }

        var normalizedEmailAddress = string.IsNullOrWhiteSpace(emailAddress) ? null : emailAddress.Trim();

        if (normalizedEmailAddress is not null && !IsValidEmailAddress(normalizedEmailAddress))
        {
            throw new ArgumentException("The email address must be valid.", nameof(emailAddress));
        }

        var normalizedMobilePhoneNumber = string.IsNullOrWhiteSpace(mobilePhoneNumber) ? null : mobilePhoneNumber.Trim();

        if (normalizedMobilePhoneNumber is not null && !IsValidMobilePhoneNumber(normalizedMobilePhoneNumber))
        {
            throw new ArgumentException("The mobile phone number must use E.164 format.", nameof(mobilePhoneNumber));
        }

        Abstractions.Rules.BusinessRuleValidator.Validate(new Rules.PreferredCultureMustBeSupportedRule(normalizedPreferredCulture));

        return new Person(
            id,
            givenName.Trim(),
            firstSurname.Trim(),
            string.IsNullOrWhiteSpace(secondSurname) ? null : secondSurname.Trim(),
            birthDate,
            normalizedPreferredCulture,
            identityDocumentType,
            documentNumber.Trim(),
            sex,
            nationality,
            normalizedEmailAddress,
            normalizedMobilePhoneNumber);
    }

    private static bool IsValidEmailAddress(string emailAddress)
    {
        var atIndex = emailAddress.IndexOf('@');

        return atIndex > 0 &&
            atIndex == emailAddress.LastIndexOf('@') &&
            atIndex < emailAddress.Length - 1 &&
            !emailAddress.Any(char.IsWhiteSpace);
    }

    private static bool IsValidMobilePhoneNumber(string mobilePhoneNumber)
    {
        return mobilePhoneNumber.Length is >= 8 and <= 16 &&
            mobilePhoneNumber[0] == '+' &&
            mobilePhoneNumber[1..].All(char.IsDigit);
    }
}
