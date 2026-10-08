using ResearchAtlas.Configuration;
using ResearchAtlas.DependencyInjection;
using ResearchAtlas.Worker;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, configuration) =>
        configuration.AddSharedSettings(context.HostingEnvironment.IsDevelopment()))
    .UseServiceProviderFactory(
        context => new ResearchAtlasServiceProviderFactory(context.HostingEnvironment.IsDevelopment()))
    .ConfigureServices(services =>
    {
        services.AddSerilog(loggerConfiguration => loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
