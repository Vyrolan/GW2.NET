// <copyright file="ApiQuery.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Connectivity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>Represents information about a queryable location.</summary>
    public sealed class ApiQuery
    {
        /// <summary>Gets or sets a resource's location.</summary>
        public Uri ResourceLocation { get; set; }

        /// <summary>Gets or sets the query's language.</summary>
        public CultureInfo Language { get; set; }

        /// <summary>Gets or sets a query's list of identifiers.</summary>
        public IEnumerable<string> Identifiers { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
