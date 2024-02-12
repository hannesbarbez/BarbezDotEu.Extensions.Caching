using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching.Interfaces
{
    /// <summary>
    /// Encapsulates <see cref="IMemoryCache"/> in order to ensure a more rigid and structured system of caching keys.
    /// </summary>
    public interface IEncapsulatedMemoryCache
    {
        /// <summary>
        /// Creates a cache entry.
        /// </summary>
        /// <typeparam name="TCaller">The type where the caching is for or originates from.</typeparam>
        /// <typeparam name="TCachable">The type of the object to cache.</typeparam>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <param name="cachable">The object to cache.</param>
        /// <param name="options">The existing <see cref="MemoryCacheEntryOptions"/> instance to apply to the new entry.</param>
        void Set<TCachable, TCaller>(string method, string differentiator, TCachable cachable, MemoryCacheEntryOptions options);

        /// <summary>
        /// Gets an object from cache.
        /// </summary>
        /// <typeparam name="TReturn">The return type.</typeparam>
        /// <typeparam name="TCaller">The type where the caching is for or originates from.</typeparam>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <returns>The cached object, if any. Null if method and differentiator combined are not part of any caching key.</returns>
        TReturn Get<TReturn, TCaller>(string method, string differentiator)
            where TReturn : class;
    }
}