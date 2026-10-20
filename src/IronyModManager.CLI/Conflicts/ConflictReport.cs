// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ConflictReport.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using IronyModManager.CLI.Input;

namespace IronyModManager.CLI.Conflicts
{
    /// <summary>
    /// One definition taking part in a conflict.
    /// </summary>
    /// <param name="Mod">The mod name.</param>
    /// <param name="File">The file relative to the mod root.</param>
    /// <param name="IsFromGame">Whether the definition comes from the base game.</param>
    public record DefinitionRef(string Mod, string File, bool IsFromGame);

    /// <summary>
    /// A conflicting definition key and the definition that ends up in the game.
    /// </summary>
    /// <param name="TypeAndId">The type and identifier.</param>
    /// <param name="Type">The type.</param>
    /// <param name="Id">The identifier.</param>
    /// <param name="ValueType">The value type.</param>
    /// <param name="PriorityType">Why the winner wins: FIOS, LIOS, ModOrder, ModOverride or None.</param>
    /// <param name="Winner">The winner.</param>
    /// <param name="Definitions">The definitions in Irony's evaluated order.</param>
    public record ConflictEntry(string TypeAndId, string Type, string Id, string ValueType, string PriorityType, DefinitionRef Winner, IReadOnlyList<DefinitionRef> Definitions);

    /// <summary>
    /// The result of a conflict analysis.
    /// </summary>
    /// <param name="Game">The game.</param>
    /// <param name="Mods">The analyzed mods in load order.</param>
    /// <param name="Unmatched">The entries that matched no installed mod.</param>
    /// <param name="Conflicts">The conflicts.</param>
    public record ConflictReport(string Game, IReadOnlyList<string> Mods, IReadOnlyList<PlaylistEntry> Unmatched, IReadOnlyList<ConflictEntry> Conflicts);
}
