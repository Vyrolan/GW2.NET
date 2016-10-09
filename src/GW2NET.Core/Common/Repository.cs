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
            Result<TDataContract> queryResult = await this.Connector.QueryAsync<TDataContract>(this.QueryExpression, cancellationToken);

            return this.Converter.Convert(queryResult.Data, queryResult.State);
        }
    }
}