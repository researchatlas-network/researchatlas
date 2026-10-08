namespace ResearchAtlas.Domain.Enums;

public static class ApplicationStatusExtensions
{
    public static bool IsFinal(this ApplicationStatus status)
    {
        return status is ApplicationStatus.ClosedWithoutResolution
            or ApplicationStatus.Positive
            or ApplicationStatus.Negative
            or ApplicationStatus.AppealAccepted
            or ApplicationStatus.AppealRejected;
    }
}
