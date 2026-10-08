using ResearchAtlas.Domain.Abstractions.Rules;

namespace ResearchAtlas.Domain.Rules;

public sealed class PreferredCultureMustBeSupportedRule : IBusinessRule
{
    private readonly string _preferredCulture;

    public PreferredCultureMustBeSupportedRule(string preferredCulture)
    {
        _preferredCulture = preferredCulture;
    }

    public string RuleName => "PreferredCulture.Supported";

    public string ErrorMessage => "Preferred culture must be es-ES, ca-ES, or en-GB.";

    public bool IsBroken() => _preferredCulture is not ("es-ES" or "ca-ES" or "en-GB");

    public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default) => Task.FromResult(IsBroken());
}
