﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateListingPageRequest.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Represents a page request that targets the /v2/commerce/prices interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.V2.Commerce.Prices
{
    using GW2NET.V2.Common;

    /// <summary>Represents a page request that targets the /v2/commerce/prices interface.</summary>
    internal sealed class AggregateListingPageRequest : PageRequest
    {
        /// <summary>Gets the resource path.</summary>
        public override string Resource
        {
            get
            {
                return "/v2/commerce/prices";
            }
        }
    }
}