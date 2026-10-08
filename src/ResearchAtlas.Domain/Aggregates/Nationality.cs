namespace ResearchAtlas.Domain.Aggregates;

public sealed class Nationality
{
    internal Nationality(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    public Guid Id { get; }

    public string Code { get; }

    public string Name { get; }
}
