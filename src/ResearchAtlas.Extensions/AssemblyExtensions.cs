using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ResearchAtlas.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Assembly"/> class.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Reads the content of an embedded resource from the specified assembly asynchronously.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<string> ReadEmbeddedResourceAsync(this Assembly assembly, string resourceName, CancellationToken cancellationToken)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Could not retrieve the embedded resource: {resourceName}");
            }

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken);
        }
    }
}
