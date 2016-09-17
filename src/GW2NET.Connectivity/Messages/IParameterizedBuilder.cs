﻿// <copyright file="IParameterizedBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Provides the interface to add parameters to a <see cref="System.Net.Http.HttpRequestMessage"/>.</summary>
    public interface IParameterizedBuilder : IBaseBuilder
    {
        /// <summary>Sets an arbitrary parameter.</summary>
        /// <param name="key">The parameter name.</param>
        /// <param name="value">The parameter value</param>
        /// <returns>An <see cref="IParameterizedBuilder"/> used to add further parameters.</returns>
        IParameterizedBuilder WithParameter(string key, string value);

        /// <summary>Sets the culture of a request.</summary>
        /// <param name="culture">The <see cref="CultureInfo"/> for the request.</param>
        /// <returns>An <see cref="IParameterizedBuilder"/> used to add further parameters.</returns>
        IParameterizedBuilder ForCulture(CultureInfo culture);

        /// <summary>Sets a single identifier for the request.</summary>
        /// <param name="identifier">The identifier toi set.</param>
        /// <typeparam name="TKey">The itentifiers type.</typeparam>
        /// <returns>A <see cref="IBaseBuilder"/> used to build the request.</returns>
        IBaseBuilder WithIdentifier<TKey>(TKey identifier);

        /// <summary>Sets a set of identifiers for the request.</summary>
        /// <param name="identifiers">An <see cref="IEnumerable{T}"/> of type <see cref="int"/> use as identifiers.</param>
        /// <typeparam name="TKey">The itentifiers type.</typeparam>
        /// <returns>A <see cref="IBaseBuilder"/> used to build the request.</returns>
        IBaseBuilder WithIdentifiers<TKey>(IEnumerable<TKey> identifiers);

        /// <summary>Sets a quantity used in the request.</summary>
        /// <param name="quantity">The quantity.</param>
        /// <returns>A <see cref="IBaseBuilder"/> used to build the request.</returns>
        IBaseBuilder WithQuantity(int quantity);

        /// <summary>Sets the page the <see cref="System.Net.Http.HttpRequestMessage"/> should request.</summary>
        /// <param name="pageIndex">The zero based page index.</param>
        /// <returns>A <see cref="IBaseBuilder"/> used to build the request.</returns>
        IPagedBuilder OnPage(int pageIndex);
    }
}