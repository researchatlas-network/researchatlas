namespace ResearchAtlas.Domain.Aggregates;

public sealed class Tenant
{
    internal Tenant(Guid id, string name, string slug)
    {
        Id = id;
        Name = name;
        Slug = slug;
        IsActive = true;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string Slug { get; }

    public bool IsActive { get; private set; }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
