using ResearchAtlas.Domain.Aggregates;

namespace ResearchAtlas.Domain.Entities;

public sealed class CallResolutionTemplate
{
    internal CallResolutionTemplate(Enums.ApplicationStatus finalStatus, ResolutionTemplate resolutionTemplate)
    {
        FinalStatus = finalStatus;
        ResolutionTemplate = resolutionTemplate;
    }

    public Enums.ApplicationStatus FinalStatus { get; }

    public ResolutionTemplate ResolutionTemplate { get; private set; }

    internal void ChangeResolutionTemplate(ResolutionTemplate resolutionTemplate)
    {
        ResolutionTemplate = resolutionTemplate;
    }
}
