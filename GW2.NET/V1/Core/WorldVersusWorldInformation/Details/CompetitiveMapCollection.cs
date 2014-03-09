﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompetitiveMapCollection.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace GW2DotNET.V1.Core.WorldVersusWorldInformation.Details
{
    /// <summary>
    ///     Represents a collection of World versus World maps.
    /// </summary>
    public class CompetitiveMapCollection : JsonList<CompetitiveMap>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CompetitiveMapCollection" /> class.
        /// </summary>
        public CompetitiveMapCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompetitiveMapCollection" /> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public CompetitiveMapCollection(IEnumerable<CompetitiveMap> collection)
            : base(collection)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompetitiveMapCollection" /> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public CompetitiveMapCollection(int capacity)
            : base(capacity)
        {
        }
    }
}