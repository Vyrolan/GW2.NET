// <copyright file="RepositoryExtensions.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Connectivity;

    /// <summary>Provides a set of extensions methods for api querying.</summary>
    public static class RepositoryExtensions
    {
       /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifiers">An <see cref="IEnumerable{T}"/> of type <see cref="TKey"/> used to identify the items.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, IEnumerable<TKey> identifiers)
        {
            return GetAsync(repository, identifiers, CancellationToken.None);
        }

        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifiers">An <see cref="IEnumerable{T}"/> of type <see cref="TKey"/> used to identify the items.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static async Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, IEnumerable<TKey> identifiers, CancellationToken cancellationToken)
        {
            // Split the id list into a set of sets with |S| <= 200
            IEnumerable<IEnumerable<TKey>> idListList = CalculatePages(identifiers);

            // Query the api asnychronously with each set of ids and await all
            Result<TDataContract>[] results = await Task.WhenAll(idListList.Select(idList => GetItemsFromApiAsync(idList, repository, cancellationToken)));

            // Flatten the collections and return the converted elements
            return results.SelectMany(result => result.Data, (result, contract) => repository.Converter.Convert(contract, result.State));
        }

        /// <summary>Calls the Guild Wars 2 Api and gets the items with the specified ids.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="idList">The list of ids to query.</param>
        /// <param name="repository">The repository containing client and converters</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="ISlice{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        private static Task<Result<TDataContract>> GetItemsFromApiAsync<TKey, TDataContract, TValue>(IEnumerable<TKey> idList, IRepository<TDataContract, TValue> repository, CancellationToken cancellationToken)
        {
            ApiQuery query = new ApiQuery
            {
                ResourceLocation = repository.Query
            };

            // ReSharper disable once SuspiciousTypeConversion.Global
            // Can be done here, since this is precisely the check we need.
            ILocalizable localizableRepository = repository as ILocalizable;
            if (localizableRepository != null)
            {
                query.Language = localizableRepository.Culture;
            }

            query.Identifiers = idList.Select(i => i.ToString());

            return repository.Connector.QueryAsync<TDataContract>(repository.q, cancellationToken);
        }

        /// <summary>Creates a set of sets for api querying.</summary>
        /// <typeparam name="TKey">The type of keys in the set.</typeparam>
        /// <param name="identifiers">The identifiers to split.</param>
        /// <returns>A set containing a set with up to 200 ids to query the Guild Wars 2 api.</returns>
        private static IEnumerable<IEnumerable<TKey>> CalculatePages<TKey>(IEnumerable<TKey> identifiers)
        {
            IList<TKey> idList = identifiers.ToList();
            IList<IList<TKey>> returnList = new List<IList<TKey>>();

            int setCount = idList.Count / 200;
            int setRemainder = idList.Count % 200;

            for (int i = 0; i < setCount; i++)
            {
                returnList.Add(idList.Skip(200 * i).Take(200).ToList());
            }

            if (setRemainder > 0)
            {
                returnList.Add(idList.Skip(200 * setCount).Take(setRemainder).ToList());
            }

            return returnList;
        }
    }
}