using ResearchAtlas.Domain.Enums;
using ResearchAtlas.Domain.Factories;
using Xunit;

namespace ResearchAtlas.Domain.Tests;

public sealed class TrainingSessionTests
{
    [Fact]
    public void AddAttendance_AllowsRemoteAndInPersonAttendeesForTheSameSession()
    {
        var session = TrainingSessionFactory.Create(
            Guid.CreateVersion7(),
            "Evaluator onboarding",
            new DateOnly(2026, 10, 15),
            physicalLocation: "Main campus, Room 204");
        var inPersonPerson = CreatePerson();
        var remotePerson = CreatePerson();

        session.AddAttendance(Guid.CreateVersion7(), inPersonPerson, TrainingAttendanceMode.InPerson);
        session.AddAttendance(Guid.CreateVersion7(), remotePerson, TrainingAttendanceMode.Remote);

        var attendances = session.Attendances.ToList();
        Assert.Equal(2, attendances.Count);
        Assert.Equal(TrainingAttendanceMode.InPerson, attendances[0].Mode);
        Assert.Equal(TrainingAttendanceMode.Remote, attendances[1].Mode);
    }

    [Fact]
    public void AddAttendance_RejectsInPersonAttendanceWithoutAPhysicalLocation()
    {
        var session = TrainingSessionFactory.Create(
            Guid.CreateVersion7(),
            "Remote evaluator onboarding",
            new DateOnly(2026, 10, 15));

        var exception = Assert.Throws<InvalidOperationException>(() => session.AddAttendance(
            Guid.CreateVersion7(),
            CreatePerson(),
            TrainingAttendanceMode.InPerson));

        Assert.Equal("An in-person attendance requires a physical training location.", exception.Message);
    }

    [Fact]
    public void AddAttendance_RejectsDuplicatePersonAttendance()
    {
        var session = TrainingSessionFactory.Create(
            Guid.CreateVersion7(),
            "Evaluator onboarding",
            new DateOnly(2026, 10, 15));
        var person = CreatePerson();

        session.AddAttendance(Guid.CreateVersion7(), person, TrainingAttendanceMode.Remote);

        var exception = Assert.Throws<ArgumentException>(() => session.AddAttendance(
            Guid.CreateVersion7(),
            person,
            TrainingAttendanceMode.Remote));

        Assert.Equal("person", exception.ParamName);
    }

    private static ResearchAtlas.Domain.Aggregates.Person CreatePerson()
    {
        return PersonFactory.Create(
            Guid.CreateVersion7(),
            "Ada",
            "Lovelace",
            null,
            new DateOnly(1815, 12, 10),
            "en-GB",
            IdentityDocumentType.Passport,
            "12345678",
            Sex.Female);
    }
}
