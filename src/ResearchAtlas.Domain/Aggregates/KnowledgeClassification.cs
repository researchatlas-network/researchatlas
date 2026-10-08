using ResearchAtlas.Domain.Entities;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class KnowledgeClassification
{
    private readonly List<KnowledgeScope> _scopes = [];

    internal KnowledgeClassification(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }

    public IReadOnlyCollection<KnowledgeScope> Scopes => _scopes.AsReadOnly();

    public void AddScope(Guid id, string code, string name)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var normalizedCode = code.Trim();

        if (_scopes.Any(scope => string.Equals(scope.Code, normalizedCode, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("A knowledge scope with the same code already exists.", nameof(code));
        }

        _scopes.Add(new KnowledgeScope(id, normalizedCode, name.Trim()));
    }

    public void AddField(Guid scopeId, Guid id, string code, string name)
    {
        var scope = FindScope(scopeId);
        scope.AddField(id, code, name);
    }

    public void AddArea(Guid fieldId, Guid id, string code, string name)
    {
        var field = _scopes.SelectMany(scope => scope.Fields).SingleOrDefault(field => field.Id == fieldId);

        if (field is null)
        {
            throw new ArgumentException("The knowledge field does not belong to this classification.", nameof(fieldId));
        }

        field.AddArea(id, code, name);
    }

    private KnowledgeScope FindScope(Guid scopeId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(scopeId, Guid.Empty);

        var scope = _scopes.SingleOrDefault(scope => scope.Id == scopeId);

        if (scope is null)
        {
            throw new ArgumentException("The knowledge scope does not belong to this classification.", nameof(scopeId));
        }

        return scope;
    }
}
