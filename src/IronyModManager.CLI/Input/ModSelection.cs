// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ModSelection.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using IronyModManager.Models.Common;

namespace IronyModManager.CLI.Input
{
    /// <summary>
    /// The installed mods to analyze in load order, plus the playlist entries that matched no installed mod.
    /// </summary>
    /// <param name="Mods">The mods.</param>
    /// <param name="Unmatched">The unmatched entries.</param>
    public record ModSelection(IReadOnlyList<IMod> Mods, IReadOnlyList<PlaylistEntry> Unmatched);
}
