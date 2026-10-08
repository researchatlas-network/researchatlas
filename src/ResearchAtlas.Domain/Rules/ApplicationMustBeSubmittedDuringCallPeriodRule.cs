using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationMustBeSubmittedDuringCallPeriodRule(
    DateTimeOffset submittedAtUtc,
    DateTimeOffset applicationPeriodStartsAtUtc,
    DateTimeOffset applicationPeriodEndsAtUtc) : IBusinessRule
{
    public string RuleName => "Application.SubmissionWithinCallPeriod";

    public string ErrorMessage => "Applications can only be submitted during the call application period.";

    public bool IsBroken() => submittedAtUtc < applicationPeriodStartsAtUtc || submittedAtUtc > applicationPeriodEndsAtUtc;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
