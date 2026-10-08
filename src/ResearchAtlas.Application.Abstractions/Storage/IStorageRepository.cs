namespace ResearchAtlas.Application.Abstractions.Storage;

public interface IStorageRepository
{
    Task<Stream?> OpenReadAsync(string key, CancellationToken cancellationToken = default);

    Task SaveAsync(string key, Stream content, CancellationToken cancellationToken = default);

    Task DeleteAsync(string key, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}
