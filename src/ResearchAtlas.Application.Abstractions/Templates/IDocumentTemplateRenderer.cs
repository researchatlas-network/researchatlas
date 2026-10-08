using System.Globalization;

namespace ResearchAtlas.Application.Abstractions.Templates;

public interface IDocumentTemplateRenderer
{
    Task<string> RenderAsync<TModel>(
        string templateName,
        TModel model,
        CultureInfo culture,
        CancellationToken cancellationToken = default);
}
