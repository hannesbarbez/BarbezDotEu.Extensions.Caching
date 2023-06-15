using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching.Interfaces
{
    /// <summary>
    /// Encapsulates <see cref="IMemoryCache"/> in order to ensure a more rigid and structured system of caching keys.
    /// </summary>
    public interface IEncapsulatedMemoryCache
    {
        /// <summary>
        /// Gets or creates a cache entry.
        /// </summary>
        /// <typeparam name="T">The name of the class where the caching is for or originates from.</typeparam>
        /// <param name="method">The name of the method to use as part of the caching key.</param>
        /// <param name="differentiator">The name of the differentiator to use as part of the caching key.</param>
        /// <param name="cachable">The object to cache.</param>
        void GetOrCreate<T>(string method, string differentiator, object cachable);
    }
}