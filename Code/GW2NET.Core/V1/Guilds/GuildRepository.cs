﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuildRepository.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Represents a repository that retrieves data from the /v1/guild_details.json interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.V1.Guilds
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Common;
    using GW2NET.Entities.Guilds;
    using GW2NET.V1.Guilds.Json;
    using GW2NET.V1.Guilds.Json.Converters;

    /// <summary>Represents a repository that retrieves data from the /v1/guild_details.json interface.</summary>
    public class GuildRepository : IRepository<Guid, Guild>, IRepository<string, Guild>
    {
        /// <summary>Infrastructure. Holds a reference to a type converter.</summary>
        private readonly IConverter<GuildDataContract, Guild> converterForGuild;

        /// <summary>Infrastructure. Holds a reference to the service client.</summary>
        private readonly IServiceClient serviceClient;

        /// <summary>Initializes a new instance of the <see cref="GuildRepository"/> class.</summary>
        /// <param name="serviceClient">The service client.</param>
        public GuildRepository(IServiceClient serviceClient)
            : this(serviceClient, new ConverterForGuild())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GuildRepository"/> class.</summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="converterForGuild">The converter for <see cref="Guild"/>.</param>
        internal GuildRepository(IServiceClient serviceClient, IConverter<GuildDataContract, Guild> converterForGuild)
        {
            Contract.Requires(serviceClient != null);
            Contract.Requires(converterForGuild != null);
            Contract.Ensures(this.serviceClient != null);
            Contract.Ensures(this.converterForGuild != null);
            this.serviceClient = serviceClient;
            this.converterForGuild = converterForGuild;
        }

        /// <inheritdoc />
        ICollection<Guid> IDiscoverable<Guid>.Discover()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollection<Guid>> IDiscoverable<Guid>.DiscoverAsync()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollection<Guid>> IDiscoverable<Guid>.DiscoverAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Guild IRepository<Guid, Guild>.Find(Guid identifier)
        {
            var request = new GuildRequest { GuildId = identifier };
            var response = this.serviceClient.Send<GuildDataContract>(request);
            if (response.Content == null)
            {
                return null;
            }

            return this.converterForGuild.Convert(response.Content);
        }

        /// <inheritdoc />
        IDictionaryRange<Guid, Guild> IRepository<Guid, Guild>.FindAll()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        IDictionaryRange<Guid, Guild> IRepository<Guid, Guild>.FindAll(ICollection<Guid> identifiers)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<Guid, Guild>> IRepository<Guid, Guild>.FindAllAsync()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<Guid, Guild>> IRepository<Guid, Guild>.FindAllAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<Guid, Guild>> IRepository<Guid, Guild>.FindAllAsync(ICollection<Guid> identifiers)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<Guid, Guild>> IRepository<Guid, Guild>.FindAllAsync(ICollection<Guid> identifiers, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<Guild> IRepository<Guid, Guild>.FindAsync(Guid identifier)
        {
            IRepository<Guid, Guild> self = this;
            return self.FindAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        Task<Guild> IRepository<Guid, Guild>.FindAsync(Guid identifier, CancellationToken cancellationToken)
        {
            var request = new GuildRequest { GuildId = identifier };
            return this.serviceClient.SendAsync<GuildDataContract>(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;
                if (response.Content == null)
                {
                    return null;
                }

                return this.converterForGuild.Convert(response.Content);
            }, cancellationToken);
        }

        /// <inheritdoc />
        ICollectionPage<Guild> IPaginator<Guild>.FindPage(int pageIndex)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        ICollectionPage<Guild> IPaginator<Guild>.FindPage(int pageIndex, int pageSize)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollectionPage<Guild>> IPaginator<Guild>.FindPageAsync(int pageIndex)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollectionPage<Guild>> IPaginator<Guild>.FindPageAsync(int pageIndex, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollectionPage<Guild>> IPaginator<Guild>.FindPageAsync(int pageIndex, int pageSize)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollectionPage<Guild>> IPaginator<Guild>.FindPageAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        ICollection<string> IDiscoverable<string>.Discover()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollection<string>> IDiscoverable<string>.DiscoverAsync()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<ICollection<string>> IDiscoverable<string>.DiscoverAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Guild IRepository<string, Guild>.Find(string identifier)
        {
            var request = new GuildRequest { GuildName = identifier };
            var response = this.serviceClient.Send<GuildDataContract>(request);
            if (response.Content == null)
            {
                return null;
            }

            return this.converterForGuild.Convert(response.Content);
        }

        /// <inheritdoc />
        IDictionaryRange<string, Guild> IRepository<string, Guild>.FindAll()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        IDictionaryRange<string, Guild> IRepository<string, Guild>.FindAll(ICollection<string> identifiers)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<string, Guild>> IRepository<string, Guild>.FindAllAsync()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<string, Guild>> IRepository<string, Guild>.FindAllAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<string, Guild>> IRepository<string, Guild>.FindAllAsync(ICollection<string> identifiers)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<IDictionaryRange<string, Guild>> IRepository<string, Guild>.FindAllAsync(ICollection<string> identifiers, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        Task<Guild> IRepository<string, Guild>.FindAsync(string identifier)
        {
            return ((IRepository<string, Guild>)this).FindAsync(identifier, CancellationToken.None);
        }
        
        /// <inheritdoc />
        Task<Guild> IRepository<string, Guild>.FindAsync(string identifier, CancellationToken cancellationToken)
        {
            var request = new GuildRequest { GuildName = identifier };
            return this.serviceClient.SendAsync<GuildDataContract>(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;
                if (response.Content == null)
                {
                    return null;
                }

                return this.converterForGuild.Convert(response.Content);
            }, cancellationToken);
        }

        /// <summary>The invariant method for this class.</summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.serviceClient != null);
            Contract.Invariant(this.converterForGuild != null);
        }
    }
}