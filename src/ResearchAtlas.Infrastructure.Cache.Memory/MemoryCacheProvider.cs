using Microsoft.Extensions.Caching.Memory;
using ResearchAtlas.Application.Abstractions.Caching;

namespace ResearchAtlas.Infrastructure.Cache.Memory
{
    public sealed class MemoryCacheProvider(IMemoryCache memoryCache) : ICacheProvider
    {
        public T? Get<T>(string key)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            return memoryCache.Get<T>(key);
        }

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(Get<T>(key));
        }

        public void Set<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            var options = new MemoryCacheEntryOptions();

            if (absoluteExpirationRelativeToNow.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            }

            memoryCache.Set(key, value, options);
        }

        public Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? absoluteExpirationRelativeToNow = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Set(key, value, absoluteExpirationRelativeToNow);

            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            memoryCache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Remove(key);

            return Task.CompletedTask;
        }
    }
}
