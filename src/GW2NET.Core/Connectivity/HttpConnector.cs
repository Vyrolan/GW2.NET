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
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.Serializers;
    using Provider;
    using GW2NET.Extensions;


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
        public override async Task<Result<TData>> QueryAsync<TData>(Expression query, CancellationToken cancellationToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(this.BuildQueryString(query), UriKind.Relative));

            var responseMessage = await this.httpClient.SendAsync(requestMessage, cancellationToken);

            using (var content = responseMessage.Content)
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    var contentType = content.Headers.ContentType;

                    if (contentType != null
                        && contentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        // Get the response content
                        var errorResult = await this.DeserializeAsync<ErrorResult>(content, this.errorSerializerFactory, this.gzipInflator);

                        // Get the error description, or null if none was returned
                        throw new ServiceException(errorResult?.Text);
                    }
                }

                // Get the metadata
                var metadata = this.DeserializeMetadata(responseMessage);

                // Deserialize the content
                var deserializedContent = await this.DeserializeAsync<IEnumerable<TData>>(content, this.serializerFactory, this.gzipInflator);
                var contentSlice = new Slice<TData>(deserializedContent)
                {
                    TotalCount = metadata.ResultTotal
                };

                return new Result<TData>(contentSlice, metadata);
            }
        }

        private ApiMetadata DeserializeMetadata(HttpResponseMessage responseMessage)
        {
            var headers = responseMessage.Headers;
            var content = responseMessage.Content;

            return new ApiMetadata
            {
                ContentLanguage = this.GetLanguage(content),
                ExpireDate = content.Headers.Expires ?? default(DateTimeOffset),
                RequestDate = headers.Date ?? default(DateTimeOffset),
                ResultCount = this.ReadOptionalHeader(headers, "X-Result-Count"),
                ResultTotal = this.ReadOptionalHeader(headers, "X-Result-Total")
            };
        }

        private async Task<TResult> DeserializeAsync<TResult>(HttpContent content, ISerializerFactory serializer, IConverter<Stream, Stream> compressionConverter)
        {
            Stream contentStream = new MemoryStream();

            await content.CopyToAsync(contentStream);

            var contentEncoding = content.Headers.ContentEncoding;
            if (contentEncoding != null && contentEncoding.Count > 0)
            {
                if (contentEncoding.FirstOrDefault().Equals("gzip", StringComparison.OrdinalIgnoreCase))
                {
                    var uncompressed = compressionConverter.Convert(contentStream, null);
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

        private string BuildQueryString(Expression query)
        {
            var queryEx = query as QueryExpression;
            if (queryEx == null)
            {
                throw new ArgumentNullException(nameof(query), "The query had the wrong format.");
            }

            var builder = new StringBuilder();

            var ressEnum = queryEx.Resource.OfType<ConstantExpression>().Select(e => e.Value);
            builder.Append(string.Join("/", ressEnum));

            if (queryEx.Parameters.Any())
            {
                var queryEnumerable = queryEx.Parameters.OfType<ConstantExpression>().Select(e => (KeyValuePair<string, object>)e.Value).Select(p => $"{p.Key}={p.Value}");

                builder.Append("?");
                builder.Append(string.Join("&", queryEnumerable));
            }

            return builder.ToString();
        }

        private int ReadOptionalHeader(HttpResponseHeaders headers, string headerName)
        {
            return headers.SingleOrDefault(h => string.Equals(h.Key, headerName, StringComparison.OrdinalIgnoreCase)).Value.Select(i => this.ParseToDefault(i, -1)).FirstOrDefault();
        }

        private int ParseToDefault(string input, int defaultValue)
        {
            int value;
            if (!int.TryParse(input, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        private CultureInfo GetLanguage(HttpContent content)
        {
            return content.Headers.ContentLanguage.Count == 0 ? new CultureInfo("iv") : new CultureInfo(content.Headers.ContentLanguage.First());
        }
    }
}