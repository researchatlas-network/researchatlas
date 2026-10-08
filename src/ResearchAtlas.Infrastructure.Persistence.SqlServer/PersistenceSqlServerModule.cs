using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ResearchAtlas.Application.Abstractions.Persistence;
using ResearchAtlas.Infrastructure.Persistence.SqlServer.Context;
using ResearchAtlas.Infrastructure.Persistence.SqlServer.Transactions;

namespace ResearchAtlas.Infrastructure.Persistence.SqlServer;

public sealed class PersistenceSqlServerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var configuration = context.Resolve<IConfiguration>();
            var connectionString = configuration.GetConnectionString("SqlServer")
                ?? throw new InvalidOperationException(
                    "The connection string 'ConnectionStrings:SqlServer' must be configured.");

            return new DbContextOptionsBuilder<ResearchAtlasDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        })
            .As<DbContextOptions<ResearchAtlasDbContext>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ResearchAtlasDbContext>().InstancePerLifetimeScope();
        builder.RegisterType<EfUnitOfWork>()
            .InstancePerDependency();
        builder.RegisterType<EfUnitOfWorkFactory>()
            .As<IUnitOfWorkFactory>()
            .InstancePerLifetimeScope();
    }
}