using ResearchAtlas.Application.Abstractions.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ResearchAtlas.Infrastructure.Templating.Rendering
{
    public class ScribanDocumentTemplateRenderer: IDocumentTemplateRenderer
    {
        public Task<string> RenderAsync<TModel>(
            string templateName,
            TModel model,
            CultureInfo culture,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
