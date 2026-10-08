using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class ApplicationFactory
{
    public static Application Create(Guid id, CallForApplications callForApplications, Person applicant, User createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(callForApplications);
        ArgumentNullException.ThrowIfNull(applicant);
        ArgumentNullException.ThrowIfNull(createdBy);

        return new Application(id, callForApplications, applicant, createdBy, DateTimeOffset.UtcNow);
    }
}
