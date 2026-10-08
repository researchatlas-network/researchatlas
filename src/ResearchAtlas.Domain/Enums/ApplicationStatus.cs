namespace ResearchAtlas.Domain.Enums;

public enum ApplicationStatus
{
    Draft = 1,
    Submitted = 2,
    UnderEligibilityReview = 3,
    Excluded = 4,
    Eligible = 5,
    ClosedWithoutResolution = 6,
    Positive = 7,
    Negative = 8,
    AppealAccepted = 9,
    AppealRejected = 10,
    UnderReview = 11
}
