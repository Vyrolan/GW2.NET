﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterForCraftingMaterial.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Converts objects of type <see cref="ItemDataContract" /> to objects of type <see cref="CraftingMaterial" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.V1.Items.Converters
{
    using GW2NET.Common;
    using GW2NET.Items.CraftingMaterials;
    using GW2NET.V1.Items.Json;

    /// <summary>Converts objects of type <see cref="ItemDataContract"/> to objects of type <see cref="CraftingMaterial"/>.</summary>
    internal sealed class ConverterForCraftingMaterial : IConverter<ItemDataContract, CraftingMaterial>
    {
        /// <summary>Converts the given object of type <see cref="ItemDataContract"/> to an object of type <see cref="CraftingMaterial"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public CraftingMaterial Convert(ItemDataContract value)
        {
            return new CraftingMaterial();
        }
    }
}