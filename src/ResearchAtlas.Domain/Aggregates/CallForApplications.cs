using ResearchAtlas.Domain.Entities;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Aggregates;

public sealed class CallForApplications
{
    private readonly List<CallResolutionTemplate> _resolutionTemplates = [];

    internal CallForApplications(
        Guid id,
        string title,
        CallType type,
        DateTimeOffset applicationPeriodStartsAtUtc,
        DateTimeOffset applicationPeriodEndsAtUtc)
    {
        Id = id;
        Title = title;
        Type = type;
        ApplicationPeriodStartsAtUtc = applicationPeriodStartsAtUtc;
        ApplicationPeriodEndsAtUtc = applicationPeriodEndsAtUtc;
        IsOpenForApplications = true;
    }

    public Guid Id { get; }

    public string Title { get; }

    public CallType Type { get; }

    public DateTimeOffset ApplicationPeriodStartsAtUtc { get; }

    public DateTimeOffset ApplicationPeriodEndsAtUtc { get; }

    public bool IsOpenForApplications { get; private set; }

    public IReadOnlyCollection<CallResolutionTemplate> ResolutionTemplates => _resolutionTemplates.AsReadOnly();

    public void CloseApplications()
    {
        IsOpenForApplications = false;
    }

    public void ConfigureResolutionTemplate(ApplicationStatus finalStatus, ResolutionTemplate resolutionTemplate)
    {
        ArgumentNullException.ThrowIfNull(resolutionTemplate);

        if (!finalStatus.IsFinal())
        {
            throw new ArgumentOutOfRangeException(nameof(finalStatus));
        }

        var existingTemplate = _resolutionTemplates.SingleOrDefault(template => template.FinalStatus == finalStatus);

        if (existingTemplate is null)
        {
            _resolutionTemplates.Add(new CallResolutionTemplate(finalStatus, resolutionTemplate));
            return;
        }

        existingTemplate.ChangeResolutionTemplate(resolutionTemplate);
    }

    public void EnsureResolutionTemplateConfigured(ApplicationStatus finalStatus)
    {
        var resolutionTemplate = _resolutionTemplates.SingleOrDefault(template => template.FinalStatus == finalStatus);

        if (resolutionTemplate is null)
        {
            throw new InvalidOperationException("A resolution template must be configured for the final application status.");
        }

    }
}
