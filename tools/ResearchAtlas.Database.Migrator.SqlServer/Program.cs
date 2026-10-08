using Microsoft.Extensions.Hosting;
using Serilog;

using var host = Host.CreateApplicationBuilder(args).Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting SQL Server database migration");

    // Migration execution will be added here.

    Log.Information("SQL Server database migration completed");
}
catch (Exception exception)
{
    Log.Fatal(exception, "SQL Server database migration failed");
    Environment.ExitCode = 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
