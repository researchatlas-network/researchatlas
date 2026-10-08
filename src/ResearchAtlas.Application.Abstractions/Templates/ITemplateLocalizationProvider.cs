using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ResearchAtlas.Application.Abstractions.Templates
{
    public interface ITemplateLocalizationProvider
    {
        Task<IReadOnlyDictionary<string, string>> GetAsync(
            string templateName,
            CultureInfo culture,
            CancellationToken cancellationToken = default);
    }
}
