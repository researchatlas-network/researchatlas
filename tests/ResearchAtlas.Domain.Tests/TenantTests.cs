using ResearchAtlas.Domain.Factories;
using Xunit;

namespace ResearchAtlas.Domain.Tests;

public sealed class TenantTests
{
    [Fact]
    public void Create_NormalizesTenantNameAndSlug()
    {
        var id = Guid.CreateVersion7();

        var tenant = TenantFactory.Create(id, "  Acme University  ", "  ACME-UNIVERSITY  ");

        Assert.Equal(id, tenant.Id);
        Assert.Equal("Acme University", tenant.Name);
        Assert.Equal("acme-university", tenant.Slug);
        Assert.True(tenant.IsActive);
    }

    [Theory]
    [InlineData("acme university")]
    [InlineData("-acme")]
    [InlineData("acme-")]
    [InlineData("acme_university")]
    public void Create_RejectsInvalidSlug(string slug)
    {
        var exception = Assert.Throws<ArgumentException>(() => TenantFactory.Create(Guid.CreateVersion7(), "Acme University", slug));

        Assert.Equal("slug", exception.ParamName);
    }

    [Fact]
    public void DeactivateAndActivate_UpdatesActivityStatus()
    {
        var tenant = TenantFactory.Create(Guid.CreateVersion7(), "Acme University", "acme-university");

        tenant.Deactivate();
        tenant.Activate();

        Assert.True(tenant.IsActive);
    }
}
