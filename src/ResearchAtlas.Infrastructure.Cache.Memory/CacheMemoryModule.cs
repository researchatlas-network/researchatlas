using Autofac;
using ResearchAtlas.Application.Abstractions.Caching;

namespace ResearchAtlas.Infrastructure.Cache.Memory;

public sealed class CacheMemoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MemoryCacheProvider>()
            .As<ICacheProvider>()
            .SingleInstance();
    }
}