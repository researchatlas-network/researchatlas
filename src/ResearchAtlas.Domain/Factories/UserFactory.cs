using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class UserFactory
{
    public static User Create(Guid id, Person person)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(person);

        return new User(id, person);
    }
}
