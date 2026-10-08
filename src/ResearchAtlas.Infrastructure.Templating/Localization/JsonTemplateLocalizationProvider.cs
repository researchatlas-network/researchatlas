using ResearchAtlas.Application.Abstractions.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ResearchAtlas.Infrastructure.Templating.Localization
{
    public class JsonTemplateLocalizationProvider : ITemplateLocalizationProvider
    {
        public Task<IReadOnlyDictionary<string, string>> GetAsync(
            string templateName,
            CultureInfo culture,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<string, string> localizations = new Dictionary<string, string>();

            return Task.FromResult(localizations);
        }
    }
}
