using BarbezDotEu.Extensions.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BarbezDotEu.Extensions.Caching
{
    /// <inheritdoc/>
    public class EncapsulatedMemoryCache : IEncapsulatedMemoryCache
    {
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Constructs an <see cref="EncapsulatedMemoryCache"/>.
        /// </summary>
        /// <param name="memoryCache">An instance of <see cref="IMemoryCache"/>.</param>
        public EncapsulatedMemoryCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <inheritdoc/>
        public void GetOrCreate<T>(string method, string differentiator, object cachable)
        {
            var key = $"{typeof(T).FullName}.{method}.{differentiator}";
            this.memoryCache.GetOrCreate(key, entry => cachable);
        }
    }
}
