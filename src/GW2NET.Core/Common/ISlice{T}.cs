// <copyright file="ISlice{T}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Collections.Generic;

    /// <summary>Defines a slice of a bigger collection.</summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    public interface ISlice<T> : ICollection<T>
    {
        /// <summary>Gets or sets the total number of items a collection could have.</summary>
        /// <remarks>
        /// This property makes the total number of items public.
        /// Do not confuse it with the capacity of a collection,
        /// which can still be different.
        /// </remarks>
        int TotalCount { get; set; }
    }
}