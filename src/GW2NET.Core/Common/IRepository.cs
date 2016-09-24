// <copyright file="IRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Connectivity
{
    using System;
    using Common;

    /// <summary>Describes a single api data source.</summary>
    /// <typeparam name="TDataContract">The type the api data is stored in.</typeparam>
    /// <typeparam name="TValue">The type the data is retrieved with.</typeparam>
    public interface IRepository<TDataContract, out TValue>
    {
        /// <summary>Gets the <see cref="GW2NET.Connectivity.Connector"/> used to make queries against the data source.
        /// </summary>
        Connector Connector { get; }

        /// <summary>Gets the converter used to convert between the stored type and the retrieved type.</summary>
        IConverter<TDataContract, TValue> Converter { get; }

        /// <summary>Gets the resources location.</summary>
        Uri Location { get; }
    }
}