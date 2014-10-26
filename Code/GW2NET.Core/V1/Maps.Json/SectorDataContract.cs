﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SectorDataContract.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Defines the SectorDataContract type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.V1.Maps.Json
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
    internal sealed class SectorDataContract
    {
        [DataMember(Name = "coord", Order = 3)]
        internal double[] Coordinates { get; set; }

        [DataMember(Name = "level", Order = 2)]
        internal int Level { get; set; }

        [DataMember(Name = "name", Order = 1)]
        internal string Name { get; set; }

        [DataMember(Name = "sector_id", Order = 0)]
        internal int SectorId { get; set; }
    }
}