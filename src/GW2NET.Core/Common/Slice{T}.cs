﻿// <copyright file="Slice{T}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>Represents the slice of a bigger collection.</summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    public class Slice<T> : Collection<T>, ISlice<T>
    {
        /// <summary>Initializes a new instance of the <see cref="Slice{T}"/> class.</summary>
        public Slice()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Slice{T}"/> class.</summary>
        /// <param name="items">The list that is wrapped by the new collection.</param>
        public Slice(IList<T> items)
            : base(items)
        {
        }

        /// <inhieritdoc />
        public int TotalCount { get; set; }
    }
}
