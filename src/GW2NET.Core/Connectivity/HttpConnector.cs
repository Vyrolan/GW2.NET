// <copyright file="HttpConnector.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Connectivity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Serializers;

    /// <summary>Used to make requests against a REST api.</summary>
    public sealed class HttpConnector : Connector
    {
        private readonly ISerializerFactory serializerFactory;

        private readonly ISerializerFactory errorSerializerFactory;

        private readonly IConverter<Stream, Stream> gzipInflator;

        private readonly HttpClient httpClient;

        /// <summary>Initializes a new instance of the <see cref="HttpConnector"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make requests.</param>
        /// <param name="gzipInflator">A converter to extract gzipped data.</param>
        /// <param name="errorSerializerFactory">A serializer to convert http errors into proper exceptions.</param>
        /// <param name="serializerFactory">A serializer to convert json data into proper POCOs</param>
        public HttpConnector(HttpClient httpClient, IConverter<Stream, Stream> gzipInflator, ISerializerFactory errorSerializerFactory, ISerializerFactory serializerFactory)
        {
            this.httpClient = httpClient;
            this.gzipInflator = gzipInflator;
            this.errorSerializerFactory = errorSerializerFactory;
            this.serializerFactory = serializerFactory;
        }

        /// <inheritdoc/>
        public override async Task<Result<TData>> QueryAsync<TData>(ApiQuery query, CancellationToken cancellationToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(this.BuildQueryString(query), UriKind.Relative));

            var responseMessage = await this.httpClient.SendAsync(requestMessage, cancellationToken);

            ApiMetadata metadata = this.GetMetadata(responseMessage);

            var slice = new Slice<TData> { TotalCount = metadata.ResultTotal };

            IEnumerable<TData> data = await this.GetContentAsync<IEnumerable<TData>>(responseMessage);

            foreach (TData item in data)
            {
                slice.Add(item);
            }

            return new Result<TData>(slice, metadata);
        }

        // HACK: Move to own dedicated, less error prone class
        private string BuildQueryString(ApiQuery query)
        {
            var queryString = $"{query.ResourceLocation}";
            if (query.Identifiers.Any())
            {
                queryString = queryString + $"?ids={string.Join(",", query.Identifiers)}";
            }

            if (query.Language != null)
            {
                queryString = queryString + $"?lang={query.Language.TwoLetterISOLanguageName}";
            }

            return queryString;
        }

        private async Task<TContent> GetContentAsync<TContent>(HttpResponseMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using (HttpContent content = message.Content)
            {
                if (!message.IsSuccessStatusCode)
                {
                    MediaTypeHeaderValue contentType = content.Headers.ContentType;

                    if (contentType != null
                        && contentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        // Get the response content
                        ErrorResult errorResult = await this.DeserializeAsync<ErrorResult>(content, this.errorSerializerFactory, this.gzipInflator);

                        // Get the error description, or null if none was returned
                        throw new ServiceException(errorResult?.Text);
                    }
                }

                return await this.DeserializeAsync<TContent>(content, this.serializerFactory, this.gzipInflator);
            }
        }

        private async Task<TResult> DeserializeAsync<TResult>(HttpContent content, ISerializerFactory serializer, IConverter<Stream, Stream> compressionConverter)
        {
            Stream contentStream = new MemoryStream();

            await content.CopyToAsync(contentStream);

            ICollection<string> contentEncoding = content.Headers.ContentEncoding;
            if (contentEncoding != null && contentEncoding.Count > 0)
            {
                if (contentEncoding.FirstOrDefault().Equals("gzip", StringComparison.OrdinalIgnoreCase))
                {
                    Stream uncompressed = compressionConverter.Convert(contentStream, null);
                    if (uncompressed == null)
                    {
                        throw new InvalidOperationException("Could not read stream.");
                    }

                    contentStream = uncompressed;
                }
            }

            await contentStream.FlushAsync();
            contentStream.Position = 0;
            return serializer.GetSerializer<TResult>().Deserialize(contentStream);
        }

        private ApiMetadata GetMetadata(HttpResponseMessage message)
        {
            ApiMetadata metadata = new ApiMetadata();

            using (HttpContent content = message.Content)
            {
                metadata.ContentLanguage = content.Headers.ContentLanguage.Count == 0 ? new CultureInfo("iv") : new CultureInfo(content.Headers.ContentLanguage.First());
                metadata.RequestDate = message.Headers.Date ?? default(DateTimeOffset);
                metadata.ExpireDate = content.Headers.Expires ?? default(DateTimeOffset);

                string resultTotal = message.Headers.SingleOrDefault(h => string.Equals(h.Key, "X-Result-Total", StringComparison.OrdinalIgnoreCase)).Value?.FirstOrDefault();
                metadata.ResultTotal = !string.IsNullOrEmpty(resultTotal) ? Convert.ToInt32(resultTotal) : -1;

                string resultCount = message.Headers.SingleOrDefault(h => string.Equals(h.Key, "X-Result-Count", StringComparison.OrdinalIgnoreCase)).Value?.FirstOrDefault();
                metadata.ResultCount = !string.IsNullOrEmpty(resultCount) ? Convert.ToInt32(resultCount) : -1;
            }

            return metadata;
        }
    }
}