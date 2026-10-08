using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ResearchAtlas.DependencyInjection;

public sealed class ResearchAtlasServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    private readonly bool isDevelopment;
    private readonly AutofacServiceProviderFactory factory = new();

    public ResearchAtlasServiceProviderFactory(bool isDevelopment)
    {
        this.isDevelopment = isDevelopment;
    }

    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var containerBuilder = factory.CreateBuilder(services);
        containerBuilder.RegisterModule(new ResearchAtlasContainerModule(isDevelopment));

        return containerBuilder;
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        ArgumentNullException.ThrowIfNull(containerBuilder);

        return factory.CreateServiceProvider(containerBuilder);
    }
}
