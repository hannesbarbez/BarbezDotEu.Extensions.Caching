using BarbezDotEu.Extensions.Caching.Interfaces;
using MessagePack;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BarbezDotEu.Extensions.Caching
{
    /// <summary>
    /// Implements an <see cref="IEncapsulatedMemoryCache"/> using a <see cref="GZipStream"/> to compress cached objects.
    /// </summary>
    public class EncapsulatedCompressedMemoryCache : AbstractEncapsulatedMemoryCache<KeyValuePair<string, byte[]>>, IEncapsulatedMemoryCache
    {
        private readonly MessagePackSerializerOptions messagePackOptions = MessagePackSerializerOptions.Standard
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        /// <summary>
        /// Constructs an <see cref="EncapsulatedMemoryCache"/>.
        /// </summary>
        /// <param name="memoryCache">An instance of <see cref="IMemoryCache"/>.</param>
        public EncapsulatedCompressedMemoryCache(IMemoryCache memoryCache)
            : base(memoryCache)
        {
        }

        /// <inheritdoc/>
        public void Set<TCachable, TCaller>(string method, string differentiator, TCachable value, MemoryCacheEntryOptions options)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                MessagePackSerializer.Serialize(memoryStream, value, messagePackOptions);
                base.Set<TCachable, TCaller>(method, differentiator, new KeyValuePair<string, byte[]>(
                    typeof(TCachable).AssemblyQualifiedName,
                    memoryStream.ToArray()),
                    options);
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
                    var valueType = Type.GetType(pair.Key);
                    return MessagePackSerializer.Deserialize<TReturn>(memoryStream, messagePackOptions);
                }
            }

            return default;
        }
    }
}
