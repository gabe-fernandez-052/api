using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Cache.Extensions
{
    /// <summary>
    /// Steps in Cache-aside Pattern
    /// Check Cache:The application first checks the cache for the required data.
    /// Cache Miss:If the data is not present in the cache, a cache miss occurs.The application retrieves the data from the primary data store.
    /// Populate Cache:The retrieved data is stored in the cache for future requests.
    /// Return Data:The data is returned to the application.
    /// </summary>
    public static class CacheAside
    {
        private static readonly DistributedCacheEntryOptions _defaultOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };

        /// <summary>
        /// Prevent cache stampede
        /// </summary>
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public static async Task<T?> GetOrCreateAsync<T>(this IDistributedCache cache,
                                                       string key,
                                                       Func<CancellationToken, Task<T>> creator,
                                                       DistributedCacheEntryOptions? options = null,
                                                       CancellationToken cancellationToken = default)
        {
            var cachedValue = await cache.GetStringAsync(key, cancellationToken);

            T? value;

            if (!string.IsNullOrWhiteSpace(cachedValue))
            {
                value = JsonSerializer.Deserialize<T>(cachedValue);

                if (value is not null)
                {
                    return value;
                }
            }

            var locked = await _semaphore.WaitAsync(500);

            if (!locked)
            {
                return default;
            }

            try
            {
                cachedValue = await cache.GetStringAsync(key, cancellationToken);

                if (!string.IsNullOrWhiteSpace(cachedValue))
                {
                    value = JsonSerializer.Deserialize<T>(cachedValue);

                    if (value is not null)
                    {
                        return value;
                    }
                }

                value = await creator(cancellationToken);

                if (value is null)
                {
                    return default;
                }

                await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options ?? _defaultOptions, cancellationToken);
            }
            finally
            {
                _semaphore.Release();
            }

            return value;
        }
    }
}