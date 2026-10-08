namespace ResearchAtlas.Domain.Entities;

public sealed class KnowledgeField
{
    private readonly List<KnowledgeArea> _areas = [];

    internal KnowledgeField(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    public Guid Id { get; }

    public string Code { get; }

    public string Name { get; }

    public IReadOnlyCollection<KnowledgeArea> Areas => _areas.AsReadOnly();

    internal void AddArea(Guid id, string code, string name)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var normalizedCode = code.Trim();

        if (_areas.Any(area => string.Equals(area.Code, normalizedCode, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("A knowledge area with the same code already exists in this field.", nameof(code));
        }

        _areas.Add(new KnowledgeArea(id, normalizedCode, name.Trim()));
    }
}
