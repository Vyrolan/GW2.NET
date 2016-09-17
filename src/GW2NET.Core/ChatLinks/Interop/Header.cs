// <copyright file="Header.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.ChatLinks.Interop
{
    public enum Header : byte
    {
        /// <summary>Unknown header</summary>
        Unknown = 0,

        /// <summary>Link represents a coin.</summary>
        Coin = 1,

        /// <summary>Link represents an item.</summary>
        Item = 2,

        /// <summary>Link represents a text.</summary>
        Text = 3,

        /// <summary>Link represents a map.</summary>
        Map = 4,

        /// <summary>Link represents a pvp match.</summary>
        PvP = 5,

        /// <summary>Link represents a skill.</summary>
        Skill = 7,

        /// <summary>Link represents a trait.</summary>
        Trait = 8,

        /// <summary>Link represents a player.</summary>
        Player = 9,

        /// <summary>Link represents a recipe.</summary>
        Recipe = 10,

        /// <summary>Link represents a skin.</summary>
        Skin = 11,

        /// <summary>Link represents an outfit.</summary>
        Outfit = 12
    }
}