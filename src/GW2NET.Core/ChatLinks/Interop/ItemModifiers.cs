// <copyright file="ItemModifiers.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.ChatLinks.Interop
{
    using System;

    [Flags]
    public enum ItemModifiers : byte
    {
        /// <summary>The item has no modifiers.</summary>
        None = 0,

        /// <summary>The item has a suffix</summary>
        SuffixItem = 0x40,

        /// <summary>The item has a secondary suffix.</summary>
        SecondarySuffixItem = 0x60,

        /// <summary>The item has a skin.</summary>
        Skin = 0x80
    }
}