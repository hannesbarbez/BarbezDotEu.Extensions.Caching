using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching
{
    /// <summary>
    /// Encapsulates <see cref="IMemoryCache"/> in order to ensure a more rigid and structured system of caching keys.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EncapsulatedMemoryCache<T>
    {
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Constructs an <see cref="EncapsulatedMemoryCache{T}"/>.
        /// </summary>
        /// <param name="memoryCache">An instance of <see cref="IMemoryCache"/>.</param>
        public EncapsulatedMemoryCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Gets or creates a cache entry.
        /// </summary>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <param name="cachable">The object to cache.</param>
        public void GetOrCreate(string method, string differentiator, object cachable)
        {
            var key = $"{typeof(T).FullName}.{method}.{differentiator}";
            this.memoryCache.GetOrCreate(key, entry => cachable);
        }
    }
}
