using BarbezDotEu.Extensions.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching
{
    /// <inheritdoc/>
    public class EncapsulatedMemoryCache<T> : IEncapsulatedMemoryCache
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

        /// <inheritdoc/>
        public void GetOrCreate(string method, string differentiator, object cachable)
        {
            var key = $"{typeof(T).FullName}.{method}.{differentiator}";
            this.memoryCache.GetOrCreate(key, entry => cachable);
        }
    }
}
