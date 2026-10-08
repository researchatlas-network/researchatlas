using ResearchAtlas.Domain.Entities;

namespace ResearchAtlas.Domain.Services;

public sealed class TaxWithholdingService
{
    public decimal CalculateRate(TaxProfile taxProfile, DateOnly effectiveOn)
    {
        ArgumentNullException.ThrowIfNull(taxProfile);

        return 15m;
    }
}
