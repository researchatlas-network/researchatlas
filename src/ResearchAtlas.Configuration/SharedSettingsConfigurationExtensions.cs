using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ResearchAtlas.Configuration;

public static class SharedSettingsConfigurationExtensions
{
    public static IConfigurationBuilder AddSharedSettings(
        this IConfigurationBuilder configuration,
        bool isDevelopment)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var settingsFileName = isDevelopment
            ? "SharedSettings.Development.json"
            : "SharedSettings.Production.json";
        var assembly = typeof(SharedSettingsConfigurationExtensions).Assembly;
        var resourceName = $"{assembly.GetName().Name}.{settingsFileName}";
        var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"The shared settings resource '{resourceName}' was not found.");

        // Insert first so host settings, secrets, environment variables, and arguments can override it.
        configuration.Sources.Insert(0, new JsonStreamConfigurationSource { Stream = stream });

        return configuration;
    }
}
