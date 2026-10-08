using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationCanOnlyBeSubmittedWhenDraftRule(Enums.ApplicationStatus status) : IBusinessRule
{
    public string RuleName => "Application.SubmissionRequiresDraft";

    public string ErrorMessage => "Only a draft application can be submitted.";

    public bool IsBroken() => status != Enums.ApplicationStatus.Draft;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
