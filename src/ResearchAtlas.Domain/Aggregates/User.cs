namespace ResearchAtlas.Domain.Aggregates;

public sealed class User
{
    internal User(Guid id, Person person)
    {
        Id = id;
        Person = person;
    }

    public Guid Id { get; }

    public Person Person { get; }
}
