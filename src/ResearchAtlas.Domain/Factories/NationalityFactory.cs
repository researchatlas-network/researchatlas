using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class NationalityFactory
{
    public static Nationality Create(Guid id, string code, string name)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Nationality(id, code.Trim(), name.Trim());
    }
}
