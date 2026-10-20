// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CollectionModSource.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using IronyModManager.Models.Common;
using IronyModManager.Services.Common;

namespace IronyModManager.CLI.Input
{
    /// <summary>
    /// Resolves the mods of an Irony collection.
    /// </summary>
    /// <param name="collections">The collection service.</param>
    /// <param name="mods">The mod service.</param>
    public class CollectionModSource(IModCollectionService collections, IModService mods)
    {
        #region Methods

        /// <summary>
        /// Resolves the specified collection. Mods Irony cannot find on disk are reported as unmatched.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="installed">The installed mods.</param>
        /// <returns>ModSelection.</returns>
        /// <exception cref="CliInputException">The collection does not exist.</exception>
        public ModSelection Resolve(string collectionName, IEnumerable<IMod> installed)
        {
            var collection = collections.Get(collectionName) ?? throw new CliInputException($"Collection '{collectionName}' not found");
            var resolved = mods.ResolveCollectionMods(installed, collection, null).ToList();
            var unmatched = resolved.Select((mod, index) => (mod, index)).Where(p => p.mod.IsVirtual)
                .Select(p => new PlaylistEntry(p.mod.Name, true, p.index, p.mod.RemoteId?.ToString(), null)).ToList();
            return new ModSelection(resolved.Where(p => !p.IsVirtual).ToList(), unmatched);
        }

        #endregion Methods
    }
}
