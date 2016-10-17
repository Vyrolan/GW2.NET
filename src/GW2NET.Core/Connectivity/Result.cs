// <copyright file="Result.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Connectivity
{
    /// <summary>Represents a result given back by a <see cref="Connector"/> query.</summary>
    /// <typeparam name="TData">The query's data type.</typeparam>
    public sealed class Result<TData>
    {
        /// <summary>Initializes a new instance of the <see cref="Result{TData}"/> class.</summary>
        /// <param name="data">The data to pass on.</param>
        /// <param name="state">The optional state the data was in.</param>
        public Result(TData data, object state = null)
        {
            this.Data = data;
            this.State = state;
        }

        /// <summary>Gets the data the query returned.</summary>
        public TData Data { get; }

        /// <summary>Gets the optional state the data was in when returned.</summary>
        public object State { get; }
    }
}