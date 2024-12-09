using BarbezDotEu.Extensions.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BarbezDotEu.Extensions.Caching
{
    /// <summary>
    /// Implements an <see cref="IEncapsulatedMemoryCache"/> using a <see cref="JsonSerializer"/> to cache objects in JSON.
    /// </summary>
    public class EncapsulatedMemoryCache : AbstractEncapsulatedMemoryCache<KeyValuePair<string, string>>, IEncapsulatedMemoryCache
    {
        /// <summary>
        /// Constructs an <see cref="EncapsulatedMemoryCache"/>.
        /// </summary>
        /// <param name="memoryCache">An instance of <see cref="IMemoryCache"/>.</param>
        public EncapsulatedMemoryCache(IMemoryCache memoryCache)
            :base(memoryCache) 
        {
        }

        /// <inheritdoc/>
        public void Set<TCachable, TCaller>(string method, string differentiator, TCachable value, MemoryCacheEntryOptions options)
        {
            base.Set<TCachable, TCaller>(method, differentiator, new KeyValuePair<string, string>(
                typeof(TCachable).AssemblyQualifiedName,
                JsonSerializer.Serialize(value)), options);
        }

        /// <inheritdoc/>
        public TReturn Get<TReturn, TCaller>(string method, string differentiator)
            where TReturn : class
        {
            var parsable = base.Get<TCaller>(method, differentiator);
            if (parsable is KeyValuePair<string, string> pair)
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
