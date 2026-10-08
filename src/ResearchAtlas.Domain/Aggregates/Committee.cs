using ResearchAtlas.Domain.Abstractions.Rules;
using ResearchAtlas.Domain.Entities;
using ResearchAtlas.Domain.Rules;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class Committee
{
    private readonly List<CommitteeMember> _members = [];

    internal Committee(Guid id, string name, KnowledgeScope knowledgeScope, Person chair, Person secretary)
    {
        Id = id;
        Name = name;
        KnowledgeScope = knowledgeScope;
        Chair = new CommitteeMember(Guid.CreateVersion7(), chair, Enums.CommitteeMemberRole.Chair, DateTimeOffset.UtcNow, null);
        Secretary = new CommitteeMember(Guid.CreateVersion7(), secretary, Enums.CommitteeMemberRole.Secretary, DateTimeOffset.UtcNow, null);
        _members.Add(Chair);
        _members.Add(Secretary);
        IsActive = true;
    }

    public Guid Id { get; }

    public string Name { get; }

    public KnowledgeScope KnowledgeScope { get; }

    public CommitteeMember Chair { get; private set; }

    public CommitteeMember Secretary { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset? DeactivatedAtUtc { get; private set; }

    public IReadOnlyCollection<CommitteeMember> Members => _members.AsReadOnly();

    public void AddMember(
        Guid id,
        Person person,
        Enums.CommitteeMemberRole role,
        DateTimeOffset appointedAtUtc,
        DateTimeOffset? plannedCessationAtUtc)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(person);

        if (plannedCessationAtUtc.HasValue)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(plannedCessationAtUtc.Value, appointedAtUtc);
        }

        if (!Enum.IsDefined(role))
        {
            throw new ArgumentOutOfRangeException(nameof(role));
        }

        BusinessRuleValidator.Validate(new CommitteeMemberMustNotAlreadyBelongRule(person, _members));

        if (role == Enums.CommitteeMemberRole.Chair && Chair.IsActive)
        {
            Chair.Cease(appointedAtUtc, "Replaced by a newly appointed chair.");
        }

        if (role == Enums.CommitteeMemberRole.Secretary && Secretary.IsActive)
        {
            Secretary.Cease(appointedAtUtc, "Replaced by a newly appointed secretary.");
        }

        var member = new CommitteeMember(id, person, role, appointedAtUtc, plannedCessationAtUtc);

        if (role == Enums.CommitteeMemberRole.Chair)
        {
            Chair = member;
        }

        if (role == Enums.CommitteeMemberRole.Secretary)
        {
            Secretary = member;
        }

        _members.Add(member);
    }

    public void CeaseMember(Guid memberId, DateTimeOffset ceasedAtUtc, string cessationReason)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(memberId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(cessationReason);

        var member = _members.SingleOrDefault(member => member.Id == memberId);

        if (member is null)
        {
            throw new ArgumentException("The member does not belong to this committee.", nameof(memberId));
        }

        if (member.Id == Chair.Id || member.Id == Secretary.Id)
        {
            throw new InvalidOperationException("The active chair and secretary can only be replaced by appointing a new member with the same role.");
        }

        member.Cease(ceasedAtUtc, cessationReason);
    }

    public void MarkAppointmentDocumentNotified(Guid memberId)
    {
        FindMember(memberId).MarkAppointmentDocumentNotified();
    }

    public void MarkCessationDocumentNotified(Guid memberId)
    {
        FindMember(memberId).MarkCessationDocumentNotified();
    }

    public void MarkAppreciationCertificateNotified(Guid memberId)
    {
        FindMember(memberId).MarkAppreciationCertificateNotified();
    }

    public void Deactivate(DateTimeOffset deactivatedAtUtc, string cessationReason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cessationReason);

        BusinessRuleValidator.Validate(new CommitteeCanOnlyBeDeactivatedWhenActiveRule(IsActive));

        foreach (var member in _members.Where(member => member.IsActive))
        {
            member.Cease(deactivatedAtUtc, cessationReason);
        }

        IsActive = false;
        DeactivatedAtUtc = deactivatedAtUtc;
    }

    private CommitteeMember FindMember(Guid memberId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(memberId, Guid.Empty);

        var member = _members.SingleOrDefault(member => member.Id == memberId);

        if (member is null)
        {
            throw new ArgumentException("The member does not belong to this committee.", nameof(memberId));
        }

        return member;
    }
}
