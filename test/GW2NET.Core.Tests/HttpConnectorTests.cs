namespace GW2NET.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Compression;
    using Connectivity;
    using Xunit;
    using System.Threading;
    using Common.Serializers;
    using Provider;

    public class HttpConnectorTests
    {
        [Fact]
        public async Task QueryTest()
        {
            var handler = new UnauthorizedHandler(new HttpClientHandler());
            var client = new HttpClient(handler, false)
            {
                BaseAddress = new Uri("https://render.guildwars2.com/")
            };

            var inflator = new GzipInflator();

            var serializer = new BinarySerializerFactory();
            var errorSerializer = new JsonSerializerFactory();

            var connector = new HttpConnector(client, inflator, errorSerializer, serializer);

            var query = new QueryBuilder().AtLocation("file/943538394A94A491C8632FBEF6203C2013443555/102478.png").Build();

            var result = await connector.QueryAsync<byte[]>(query, CancellationToken.None);

            Assert.NotNull(result);
        }
    }

    /// <summary>Provides factory methods for the binary serialization engine.</summary>
    public class BinarySerializerFactory : ISerializerFactory
    {
        /// <summary>Gets a serializer for the specified type.</summary>
        /// <typeparam name="T">The serialization type.</typeparam>
        /// <returns>The <see cref="ISerializer{T}"/>.</returns>
        public ISerializer<T> GetSerializer<T>()
        {
            if (typeof(byte[]) != typeof(T))
            {
                throw new NotSupportedException("The specified type is not supported by the binary serializer.");
            }

            return (ISerializer<T>) new BinarySerializer();
        }
    }
}