using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Factories;

public static class TrainingSessionFactory
{
    public static TrainingSession Create(
        Guid id,
        string title,
        DateOnly occursOn,
        string? description = null,
        string? physicalLocation = null)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new TrainingSession(
            id,
            title.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            occursOn,
            string.IsNullOrWhiteSpace(physicalLocation) ? null : physicalLocation.Trim());
    }
}
