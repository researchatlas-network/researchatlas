using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class ApplicationCanOnlyBeSubmittedWhenCallIsOpenRule(bool isOpenForApplications) : IBusinessRule
{
    public string RuleName => "Application.SubmissionRequiresOpenCall";

    public string ErrorMessage => "An application can only be submitted when its call is open for applications.";

    public bool IsBroken() => !isOpenForApplications;

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
