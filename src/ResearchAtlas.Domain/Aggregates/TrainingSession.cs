using ResearchAtlas.Domain.Entities;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class TrainingSession
{
    private readonly List<TrainingAttendance> _attendances = [];

    internal TrainingSession(Guid id, string title, string? description, DateOnly occursOn, string? physicalLocation)
    {
        Id = id;
        Title = title;
        Description = description;
        OccursOn = occursOn;
        PhysicalLocation = physicalLocation;
    }

    public Guid Id { get; }

    public string Title { get; }

    public string? Description { get; }

    public DateOnly OccursOn { get; }

    public string? PhysicalLocation { get; }

    public IReadOnlyCollection<TrainingAttendance> Attendances => _attendances.AsReadOnly();

    public void AddAttendance(Guid id, Person person, Enums.TrainingAttendanceMode mode)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(person);

        if (!Enum.IsDefined(mode))
        {
            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        if (mode == Enums.TrainingAttendanceMode.InPerson && PhysicalLocation is null)
        {
            throw new InvalidOperationException("An in-person attendance requires a physical training location.");
        }

        if (_attendances.Any(attendance => attendance.Person.Id == person.Id))
        {
            throw new ArgumentException("The person already has an attendance for this training session.", nameof(person));
        }

        _attendances.Add(new TrainingAttendance(id, person, mode));
    }
}
