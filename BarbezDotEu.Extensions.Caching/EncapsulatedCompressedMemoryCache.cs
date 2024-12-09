using BarbezDotEu.Extensions.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace BarbezDotEu.Extensions.Caching
{
    /// <summary>
    /// Implements an <see cref="IEncapsulatedMemoryCache"/> using a <see cref="GZipStream"/> to compress cached objects.
    /// </summary>
    public class EncapsulatedCompressedMemoryCache : AbstractEncapsulatedMemoryCache<KeyValuePair<string, byte[]>>, IEncapsulatedMemoryCache
    {
        private readonly BinaryFormatter binaryFormatter;

        /// <summary>
        /// Constructs an <see cref="EncapsulatedMemoryCache"/>.
        /// </summary>
        /// <param name="memoryCache">An instance of <see cref="IMemoryCache"/>.</param>
        public EncapsulatedCompressedMemoryCache(IMemoryCache memoryCache)
            : base(memoryCache)
        {
            this.binaryFormatter = new BinaryFormatter();
        }

        /// <inheritdoc/>
        public void Set<TCachable, TCaller>(string method, string differentiator, TCachable cachable, MemoryCacheEntryOptions options)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    binaryFormatter.Serialize(gZipStream, cachable);
                    base.Set<TCachable, TCaller>(method, differentiator, new KeyValuePair<string, byte[]>(
                        typeof(TCachable).AssemblyQualifiedName,
                        memoryStream.ToArray()),
                        options);
                }
            }
        }

        /// <inheritdoc/>
        public TReturn Get<TReturn, TCaller>(string method, string differentiator)
            where TReturn : class
        {
            var parsable = base.Get<TCaller>(method, differentiator);
            if (parsable is KeyValuePair<string, byte[]> pair)
            {
                using (MemoryStream memoryStream = new MemoryStream(pair.Value))
                {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        var valueType = Type.GetType(pair.Key);
                        var parsed = binaryFormatter.Deserialize(gZipStream);
                        if (parsed.GetType() == typeof(TReturn))
                        {
                            return (TReturn)parsed;
                        }
                    }
                }
            }

            return default;
        }
    }
}
