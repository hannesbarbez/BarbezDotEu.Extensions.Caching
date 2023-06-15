using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching.Interfaces
{
    /// <summary>
    /// Encapsulates <see cref="IMemoryCache"/> in order to ensure a more rigid and structured system of caching keys.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEncapsulatedMemoryCache
    {
        /// <summary>
        /// Gets or creates a cache entry.
        /// </summary>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <param name="cachable">The object to cache.</param>
        void GetOrCreate(string method, string differentiator, object cachable);
    }
}