namespace ResearchAtlas.Domain.Entities;

public sealed class KnowledgeArea
{
    internal KnowledgeArea(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    public Guid Id { get; }

    public string Code { get; }

    public string Name { get; }
}
