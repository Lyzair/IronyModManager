// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="LauncherPlaylist.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace IronyModManager.CLI.Input
{
    /// <summary>
    /// One mod entry of a Paradox Launcher playlist.
    /// </summary>
    /// <param name="DisplayName">The display name.</param>
    /// <param name="Enabled">Whether the mod is enabled.</param>
    /// <param name="Position">The load order position, lower loads first.</param>
    /// <param name="SteamId">The steam workshop identifier.</param>
    /// <param name="PdxId">The paradox mods identifier.</param>
    public record PlaylistEntry(string DisplayName, bool Enabled, int Position, string SteamId, string PdxId);

    /// <summary>
    /// A Paradox Launcher playlist export (format v3).
    /// </summary>
    /// <param name="Game">The game.</param>
    /// <param name="Name">The name.</param>
    /// <param name="Mods">The mods sorted by position.</param>
    public record LauncherPlaylist(string Game, string Name, IReadOnlyList<PlaylistEntry> Mods);
}
