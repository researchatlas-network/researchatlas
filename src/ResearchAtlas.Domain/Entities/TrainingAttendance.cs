using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class TrainingAttendance
{
    internal TrainingAttendance(Guid id, Person person, Enums.TrainingAttendanceMode mode)
    {
        Id = id;
        Person = person;
        Mode = mode;
    }

    public Guid Id { get; }

    public Person Person { get; }

    public Enums.TrainingAttendanceMode Mode { get; }
}
