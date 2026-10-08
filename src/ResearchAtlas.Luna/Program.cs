using ResearchAtlas.Configuration;
using ResearchAtlas.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSharedSettings(builder.Environment.IsDevelopment());

builder.Host.UseServiceProviderFactory(
    context => new ResearchAtlasServiceProviderFactory(context.HostingEnvironment.IsDevelopment()));

builder.Services.AddSerilog(loggerConfiguration => loggerConfiguration
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error.html");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseDefaultFiles();
if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
}


app.UseRouting();

app.UseAuthorization();

app.MapControllers();
if (!app.Environment.IsDevelopment())
{
    app.MapStaticAssets();
}

app.Run();
