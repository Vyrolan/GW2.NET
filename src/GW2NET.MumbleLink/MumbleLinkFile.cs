﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MumbleLinkFile.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Provides an implementation of the Mumble Link protocol.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.MumbleLink
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Json;
    using System.Text;

    using GW2NET.Common;
    using GW2NET.Common.Drawing;

    /// <summary>Provides an implementation of the Mumble Link protocol.</summary>
    /// <example>
    /// <code>
    /// using (var mumbler = new MumbleLinkFile())
    /// {
    ///     var avatar = mumbler.Read();
    ///     Console.WriteLine("Hello " + avatar.Identity.Name + "!");
    /// }
    /// </code>
    /// </example>
    public partial class MumbleLinkFile : IDisposable
    {
        /// <summary>Holds a reference to the shared memory block.</summary>
        private readonly MemoryMappedFile mumbleLink;

        /// <summary>The size of the shared memory block.</summary>
        private readonly int size;

        /// <summary>Indicates whether this object is disposed.</summary>
        private bool disposed;

        /// <summary>Initializes a new instance of the <see cref="MumbleLinkFile"/> class.</summary>
        public MumbleLinkFile()
        {
            this.size = Marshal.SizeOf(typeof(AvatarDataContract));
            this.mumbleLink = MemoryMappedFile.CreateOrOpen("MumbleLink", this.size, MemoryMappedFileAccess.ReadWrite);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Retrieves positional audio data from the shared memory block as defined by the Mumble Link protocol.</summary>
        /// <returns>Positional audio data as an instance of the <see cref="Avatar"/> class.</returns>
        public Avatar Read()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            using (var stream = this.mumbleLink.CreateViewStream())
            {
                // Copy the shared memory block to a local buffer in managed memory
                var buffer = new byte[this.size];
                stream.Read(buffer, 0, buffer.Length);

                // Pin the managed memory so that the GC doesn't move it
                var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                // Get the address of the managed memory
                var ptr = handle.AddrOfPinnedObject();

                AvatarDataContract avatarDataContract;

                try
                {
                    // Copy the managed memory to a managed struct
                    avatarDataContract = (AvatarDataContract)Marshal.PtrToStructure(ptr, typeof(AvatarDataContract));
                }
                finally
                {
                    // Release the handle
                    handle.Free();
                }

                // Ensure that data is available and that it was generated by Guild Wars 2
                if (!avatarDataContract.name.Equals("Guild Wars 2", StringComparison.Ordinal))
                {
                    return null;
                }

                // Convert data contracts to managed data types
                return ConvertAvatarDataContract(avatarDataContract);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <param name="disposing"><c>true</c> if managed resources should be released.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.mumbleLink.Dispose();
            }

            this.disposed = true;
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
        private static AvatarContext ConvertAvatarContextDataContract(MumbleContext context)
        {
            return new AvatarContext
                {
                    ServerAddress = ConvertServerAddress(context.serverAddress),
                    MapId = (int)context.mapId,
                    MapType = (int)context.mapType,
                    ShardId = (int)context.shardId,
                    Instance = (int)context.instance,
                    BuildId = (int)context.buildId
                };
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
        private static Avatar ConvertAvatarDataContract(AvatarDataContract dataContract)
        {
            // Copy the context data to an unmanaged memory pointer
            var contextLength = (int)dataContract.context_len;
            var ptr = Marshal.AllocHGlobal(contextLength);
            Marshal.Copy(dataContract.context, 0, ptr, contextLength);

            // Copy the unmanaged memory to a managed struct
            var mumbleContext = (MumbleContext)Marshal.PtrToStructure(ptr, typeof(MumbleContext));

            // Convert data contracts to managed data types
            // MEMO: for the first tick, only context data is available
            if (dataContract.uiTick == 1)
            {
                return new Avatar
                    {
                        UiVersion = (int)dataContract.uiVersion,
                        UiTick = dataContract.uiTick,
                        Context = ConvertAvatarContextDataContract(mumbleContext)
                    };
            }

            return new Avatar
                {
                    UiVersion = (int)dataContract.uiVersion,
                    UiTick = dataContract.uiTick,
                    Context = ConvertAvatarContextDataContract(mumbleContext),
                    Identity = ConvertIdentityDataContract(dataContract.identity),
                    AvatarFront = ConvertVector3D(dataContract.fAvatarFront),
                    AvatarTop = ConvertVector3D(dataContract.fAvatarPosition),
                    AvatarPosition = ConvertVector3D(dataContract.fAvatarPosition),
                    CameraFront = ConvertVector3D(dataContract.fCameraFront),
                    CameraPosition = ConvertVector3D(dataContract.fCameraPosition)
                };
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
        private static Identity ConvertIdentityDataContract(string identity)
        {
            var serializer = new DataContractJsonSerializer(typeof(IdentityDataContract));

            IdentityDataContract dataContract;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(identity)))
            {
                dataContract = (IdentityDataContract)serializer.ReadObject(stream);
            }

            return new Identity
                {
                    Name = dataContract.Name,
                    Profession = (Profession)dataContract.Profession,
                    Race = (Race)dataContract.Race,
                    MapId = dataContract.MapId,
                    WorldId = dataContract.WorldId,
                    TeamColorId = dataContract.TeamColorId,
                    Commander = dataContract.Commander,
                    FieldOfView = dataContract.FieldOfView
                };
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
        private static IPEndPoint ConvertServerAddress(SockaddrIn serverAddress)
        {
            return
                new IPEndPoint(
                    new IPAddress(
                        new[]
                            {
                                serverAddress.sin_addr.S_un.S_un_b.s_b1, 
                                serverAddress.sin_addr.S_un.S_un_b.s_b2, 
                                serverAddress.sin_addr.S_un.S_un_b.s_b3, 
                                serverAddress.sin_addr.S_un.S_un_b.s_b4
                            }),
                    serverAddress.sin_port);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
        private static Vector3D ConvertVector3D(float[] position)
        {
            return new Vector3D { X = position[0], Y = position[1], Z = position[2] };
        }
    }
}