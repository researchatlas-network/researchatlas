namespace ResearchAtlas.Domain.Entities;

public sealed class KnowledgeScope
{
    private readonly List<KnowledgeField> _fields = [];

    internal KnowledgeScope(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    public Guid Id { get; }

    public string Code { get; }

    public string Name { get; }

    public IReadOnlyCollection<KnowledgeField> Fields => _fields.AsReadOnly();

    internal void AddField(Guid id, string code, string name)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var normalizedCode = code.Trim();

        if (_fields.Any(field => string.Equals(field.Code, normalizedCode, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("A knowledge field with the same code already exists in this scope.", nameof(code));
        }

        _fields.Add(new KnowledgeField(id, normalizedCode, name.Trim()));
    }
}
