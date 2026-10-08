using ResearchAtlas.Domain.Aggregates;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Factories;

public static class CallForApplicationsFactory
{
    public static CallForApplications Create(
        Guid id,
        string title,
        CallType type,
        DateTimeOffset applicationPeriodStartsAtUtc,
        DateTimeOffset applicationPeriodEndsAtUtc)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type));
        }

        ArgumentOutOfRangeException.ThrowIfGreaterThan(applicationPeriodStartsAtUtc, applicationPeriodEndsAtUtc);

        return new CallForApplications(
            id,
            title.Trim(),
            type,
            applicationPeriodStartsAtUtc,
            applicationPeriodEndsAtUtc);
    }
}
