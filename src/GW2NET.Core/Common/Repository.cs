// <copyright file="Repository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Connectivity;

    /// <summary>Describes a single api data source.</summary>
    /// <typeparam name="TDataContract">The type the api data is stored in.</typeparam>
    /// <typeparam name="TValue">The type the data is retrieved with.</typeparam>
    public abstract class Repository<TDataContract, TValue>
    {
        /// <summary>Initializes a new instance of the <see cref="Repository{TDataContract,TValue}"/> class.</summary>
        /// <param name="connector">The <see cref="GW2NET.Connectivity.Connector"/> making requests against the data source.</param>
        /// <param name="converter">The <see cref="IConverter{TSource,TTarget}"/> used to convert the data source data to usable data.</param>
        protected Repository(Connector connector, IConverter<ISlice<TDataContract>, ISlice<TValue>> converter)
        {
            this.Connector = connector;
            this.Converter = converter;
        }

        /// <summary>Gets the <see cref="Connectivity.Connector"/> used to make queries against the data source.</summary>
        public Connector Connector { get; }

        /// <summary>Gets the converter used to convert between the stored type and the retrieved type.</summary>
        public IConverter<ISlice<TDataContract>, ISlice<TValue>> Converter { get; }

        /// <summary>Gets the api query.</summary>
        protected abstract Expression QueryExpression { get; }

        /// <summary>Asynchronously queries the repository.</summary>
        /// <returns>A collection of defined by this repository.</returns>
        public Task<ISlice<TValue>> QueryAsync()
        {
            return this.QueryAsync(CancellationToken.None);
        }

        /// <summary>Asynchronously queries the repository.</summary>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>A collection of defined by this repository.</returns>
        public async Task<ISlice<TValue>> QueryAsync(CancellationToken cancellationToken)
        {
            // ToDo: Fix this properly
            Result<ISlice<TDataContract>> queryResult = await this.Connector.QueryAsync<ISlice<TDataContract>>(this.QueryExpression, cancellationToken);

            return this.Converter.Convert(queryResult.Data, queryResult.State);
        }
    }
}