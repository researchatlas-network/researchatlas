using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class CommitteeMember
{
    internal CommitteeMember(
        Guid id,
        Person person,
        Enums.CommitteeMemberRole role,
        DateTimeOffset appointedAtUtc,
        DateTimeOffset? plannedCessationAtUtc)
    {
        Id = id;
        Person = person;
        Role = role;
        AppointedAtUtc = appointedAtUtc;
        PlannedCessationAtUtc = plannedCessationAtUtc;
        IsActive = true;
    }

    public Guid Id { get; }

    public Person Person { get; }

    public Enums.CommitteeMemberRole Role { get; }

    public DateTimeOffset AppointedAtUtc { get; }

    public bool IsActive { get; private set; }

    public DateTimeOffset? CeasedAtUtc { get; private set; }

    public string? CessationReason { get; private set; }

    public DateTimeOffset? PlannedCessationAtUtc { get; }

    public bool IsAppointmentDocumentNotified { get; private set; }

    public bool IsCessationDocumentNotified { get; private set; }

    public bool IsAppreciationCertificateNotified { get; private set; }

    internal void MarkAppointmentDocumentNotified()
    {
        IsAppointmentDocumentNotified = true;
    }

    internal void MarkCessationDocumentNotified()
    {
        EnsureCeased();
        IsCessationDocumentNotified = true;
    }

    internal void MarkAppreciationCertificateNotified()
    {
        EnsureCeased();
        IsAppreciationCertificateNotified = true;
    }

    internal void Cease(DateTimeOffset ceasedAtUtc, string cessationReason)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(ceasedAtUtc, AppointedAtUtc);
        ArgumentException.ThrowIfNullOrWhiteSpace(cessationReason);

        if (!IsActive)
        {
            throw new InvalidOperationException("Only an active committee member can cease.");
        }

        IsActive = false;
        CeasedAtUtc = ceasedAtUtc;
        CessationReason = cessationReason.Trim();
    }

    private void EnsureCeased()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("Only a ceased committee member can receive this notification.");
        }
    }
}
