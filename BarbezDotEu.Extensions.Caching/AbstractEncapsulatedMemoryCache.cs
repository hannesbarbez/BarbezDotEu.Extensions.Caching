using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching
{
    /// <summary>
    /// Implements base logic for encapsulating an <see cref="IMemoryCache"/>.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class AbstractEncapsulatedMemoryCache<TItem>
        where TItem : struct
    {
        /// <summary>
        /// Gets an instance of <see cref="IMemoryCache"/>.
        /// </summary>
        protected readonly IMemoryCache memoryCache;

        /// <summary>
        /// Constructs an <see cref="EncapsulatedMemoryCache"/>.
        /// </summary>
        /// <param name="memoryCache">An instance of <see cref="IMemoryCache"/>.</param>
        protected AbstractEncapsulatedMemoryCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Creates a cache entry.
        /// </summary>
        /// <typeparam name="TCaller">The type where the caching is for or originates from.</typeparam>
        /// <typeparam name="TCachable">The type of the object to cache.</typeparam>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <param name="cachable">The object to cache.</param>
        /// <param name="options">The existing <see cref="MemoryCacheEntryOptions"/> instance to apply to the new entry.</param>
        protected void Set<TCachable, TCaller>(string method, string differentiator, TItem cachable, MemoryCacheEntryOptions options)
        {
            var key = $"{typeof(TCaller).FullName}.{method}.{differentiator}";
            this.memoryCache.Set(key, cachable, options);
        }

        /// <summary>
        /// Gets an object from cache.
        /// </summary>
        /// <typeparam name="TCaller">The type where the caching is for or originates from.</typeparam>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <returns>The cached object, if any. Null if method and differentiator combined are not part of any caching key.</returns>
        protected object Get<TCaller>(string method, string differentiator)
        {
            var key = $"{typeof(TCaller).FullName}.{method}.{differentiator}";
            var parsable = this.memoryCache.Get(key);
            if (parsable == null)
                return default;

            return parsable;
        }
    }
}
