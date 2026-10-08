using ResearchAtlas.Domain.Aggregates;
using ResearchAtlas.Domain.Enums;

namespace ResearchAtlas.Domain.Factories;

public static class EvaluationProcessFactory
{
    public static EvaluationProcess CreateInitial(Guid id, Application application, Committee committee, User startedBy)
    {
        return Create(id, application, committee, EvaluationProcessType.InitialEvaluation, null, startedBy);
    }

    public static EvaluationProcess CreateAppealReview(Guid id, Application application, Committee committee, User startedBy)
    {
        return Create(id, application, committee, EvaluationProcessType.AppealReview, null, startedBy);
    }

    public static EvaluationProcess CreateReevaluation(Guid id, EvaluationProcess acceptedAppealReview, Committee committee, User startedBy)
    {
        ArgumentNullException.ThrowIfNull(acceptedAppealReview);

        if (!acceptedAppealReview.CanStartReevaluation())
        {
            throw new ArgumentException("A reevaluation requires a completed appeal review with an accepted outcome.", nameof(acceptedAppealReview));
        }

        return Create(
            id,
            acceptedAppealReview.Application,
            committee,
            EvaluationProcessType.Reevaluation,
            acceptedAppealReview,
            startedBy);
    }

    private static EvaluationProcess Create(
        Guid id,
        Application application,
        Committee committee,
        EvaluationProcessType type,
        EvaluationProcess? precedingProcess,
        User startedBy)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(committee);
        ArgumentNullException.ThrowIfNull(startedBy);

        return new EvaluationProcess(id, application, committee, type, precedingProcess, startedBy, DateTimeOffset.UtcNow);
    }
}
