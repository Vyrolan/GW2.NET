﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonLocation.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using GW2DotNET.V1.Core.Converters;
using Newtonsoft.Json;
using JsonPointConverter = GW2DotNET.V1.Core.Converters.PointConverter;

namespace GW2DotNET.V1.Core.EventDetails.Models
{
    /// <summary>
    /// Represents a polygonal location of an event on the map.
    /// </summary>
    [JsonConverter(typeof(DefaultConverter))]
    public class PolygonLocation : Location
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonLocation"/> class.
        /// </summary>
        public PolygonLocation()
            : base(LocationShape.Polygon)
        {
        }

        /// <summary>
        /// Gets or sets the series of points in the polygon.
        /// </summary>
        [JsonProperty("points", Order = 5, ItemConverterType = typeof(PointFConverter))]
        public IEnumerable<PointF> Points { get; set; }

        /// <summary>
        /// Gets or sets the location's range on the z-axis.
        /// </summary>
        [JsonProperty("z_range", Order = 4)]
        [JsonConverter(typeof(JsonPointConverter))]
        public Point ZRange { get; set; }
    }
}