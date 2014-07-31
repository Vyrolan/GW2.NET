﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfixUpgradeContract.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Represents item stats that are inherent to a specific item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.V1.Items.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GW2DotNET.Common.Contracts;

    /// <summary>Represents item stats that are inherent to a specific item.</summary>
    public sealed class InfixUpgradeContract : ServiceContract
    {
        /// <summary>Gets or sets the item's attributes.</summary>
        [DataMember(Name = "attributes", Order = 1)]
        public ICollection<ItemAttributeContract> Attributes { get; set; }

        /// <summary>Gets or sets the item's buff.</summary>
        [DataMember(Name = "buff", Order = 0)]
        public ItemBuffContract Buff { get; set; }
    }
}