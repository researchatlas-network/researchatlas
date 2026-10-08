using System.Globalization;
using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class TenantFactory
{
    public static Tenant Create(Guid id, string name, string slug)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        var normalizedSlug = slug.Trim().ToLower(CultureInfo.InvariantCulture);

        if (!IsValidSlug(normalizedSlug))
        {
            throw new ArgumentException("The tenant slug must contain only lowercase letters, numbers, and hyphens.", nameof(slug));
        }

        return new Tenant(id, name.Trim(), normalizedSlug);
    }

    private static bool IsValidSlug(string slug)
    {
        return slug.All(character => char.IsAsciiLetterOrDigit(character) || character == '-')
            && slug[0] != '-'
            && slug[^1] != '-';
    }
}
