// <copyright file="Connector.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Connectivity
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Provides methods to connect to an arbitrary data source.</summary>
    public abstract class Connector
    {
        /// <summary>Connects to the data source.</summary>
        /// <remarks><para>This is an optional method to implement. If the connection doesn't support persistance do not implement this method.
        /// If this method is overwritten a base call needn't happen, nor should it. In the base implementation this method will always return
        /// so the query logic can successfully move on.</para>
        /// <para>Under no circumstances should this method be overwritten and throw either a <see cref="System.NotSupportedException"/> or a <see cref="NotImplementedException"/>.
        /// If this is the case the standard query logic will produce runtime errors.</para></remarks>
        /// <returns>A <see cref="Task{TResult}"/> whose value indicates whether the connection was successfully established.</returns>
        public virtual Task<bool> ConnectAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>Query's the data source for the given data.</summary>
        /// <param name="query">Additional query informations.</param>
        /// <typeparam name="TData">The return data's type.</typeparam>
        /// <returns>An <see cref="Result{TData}"/> object containing the data and an optional state.</returns>
        public Task<Result<TData>> QueryAsync<TData>(Expression query)
        {
            return this.QueryAsync<TData>(query, CancellationToken.None);
        }

        /// <summary>Query's the data source for the given data.</summary>
        /// <param name="query">Additional query informations.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the current task.</param>
        /// <typeparam name="TData">The return data's type.</typeparam>
        /// <returns>An <see cref="Result{TData}"/> object containing the data and an optional state.</returns>
        public abstract Task<Result<TData>> QueryAsync<TData>(Expression query, CancellationToken cancellationToken);

        /// <summary>Disconnects from the data source.</summary>
        /// <remarks><para>This is an optional method to implement. If the connection doesn't support persistance do not implement this method.
        /// If this method is overwritten a base call needn't happen, nor should it. The query logic will call this method in a fire and forget manner.
        /// Any implementation shouldn't be requireing the caller to wait.</para>
        /// <para>Under no circumstances should this method be overwritten and throw either a <see cref="System.NotSupportedException"/> or a <see cref="NotImplementedException"/>.
        /// If this is the case the standard query logic will produce runtime errors.</para></remarks>
        /// <returns>A <see cref="Task"/> which indicated that the disconnect was successful.</returns>
        public virtual Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }
    }
}
