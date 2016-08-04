// <copyright file="UrlPosition.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    /// <summary>Enumerates the possible positions of a query parameter.</summary>
    public enum UrlPosition
    {
        /// <summary>The parameter is placed in the url.</summary>
        Url,

        /// <summary>The parameter is placed in the headers.</summary>
        Header
    }
}
