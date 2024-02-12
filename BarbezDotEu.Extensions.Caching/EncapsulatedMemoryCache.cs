using BarbezDotEu.Extensions.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text.Json;

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
        public void Set<TCachable, TCaller>(string method, string differentiator, TCachable cachable, MemoryCacheEntryOptions options)
        {
            var key = $"{typeof(TCaller).FullName}.{method}.{differentiator}";
            this.memoryCache.Set(key, new KeyValuePair<string, string>(typeof(TCachable).AssemblyQualifiedName, JsonSerializer.Serialize(cachable)), options);
        }

        /// <inheritdoc/>
        public TReturn Get<TReturn, TCaller>(string method, string differentiator)
            where TReturn : class
        {
            var key = $"{typeof(TCaller).FullName}.{method}.{differentiator}";
            var parsable = this.memoryCache.Get(key);
            if (parsable != null && parsable is KeyValuePair<string, string> pair)
            {
                var valueType = Type.GetType(pair.Key);
                var parsed = JsonSerializer.Deserialize(pair.Value, valueType);
                if (parsed.GetType() == typeof(TReturn))
                {
                    return (TReturn) parsed;
                }
            }

            return default;
        }
    }
}
