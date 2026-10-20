// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="PlaylistModMatcher.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using IronyModManager.Models.Common;

namespace IronyModManager.CLI.Input
{
    /// <summary>
    /// Matches playlist entries to installed mods.
    /// </summary>
    public static class PlaylistModMatcher
    {
        #region Methods

        /// <summary>
        /// Matches enabled playlist entries by steam id, falling back to the display name for local mods.
        /// </summary>
        /// <param name="installed">The installed mods.</param>
        /// <param name="playlist">The playlist.</param>
        /// <returns>ModSelection.</returns>
        public static ModSelection Match(IEnumerable<IMod> installed, LauncherPlaylist playlist)
        {
            var installedMods = installed.ToList();
            var mods = new List<IMod>();
            var unmatched = new List<PlaylistEntry>();
            foreach (var entry in playlist.Mods.Where(p => p.Enabled))
            {
                IMod mod = null;
                if (long.TryParse(entry.SteamId, out var steamId))
                {
                    mod = installedMods.FirstOrDefault(p => p.RemoteId == steamId);
                }

                mod ??= installedMods.FirstOrDefault(p => !p.RemoteId.HasValue && string.Equals(p.Name, entry.DisplayName, StringComparison.Ordinal));
                if (mod != null)
                {
                    mods.Add(mod);
                }
                else
                {
                    unmatched.Add(entry);
                }
            }

            return new ModSelection(mods, unmatched);
        }

        #endregion Methods
    }
}
