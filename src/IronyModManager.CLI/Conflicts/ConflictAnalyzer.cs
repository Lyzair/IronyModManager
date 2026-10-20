// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ConflictAnalyzer.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;
using System.Threading.Tasks;
using IronyModManager.CLI.Input;
using IronyModManager.Models.Common;
using IronyModManager.Services.Common;

namespace IronyModManager.CLI.Conflicts
{
    /// <summary>
    /// Runs the same pipeline as the GUI's read-only conflict solver.
    /// Implements the <see cref="IronyModManager.CLI.Conflicts.IConflictAnalyzer" />
    /// </summary>
    /// <param name="games">The game service.</param>
    /// <param name="languages">The game language service.</param>
    /// <param name="patches">The patch collection service.</param>
    /// <param name="gameIndex">The game index service.</param>
    /// <seealso cref="IronyModManager.CLI.Conflicts.IConflictAnalyzer" />
    public class ConflictAnalyzer(IGameService games, IGameLanguageService languages, IModPatchCollectionService patches, IGameIndexService gameIndex) : IConflictAnalyzer
    {
        #region Methods

        /// <summary>
        /// Analyzes the selection.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="selection">The selection.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="includeGame">if set to <c>true</c> the base game's definitions take part.</param>
        /// <returns>ConflictReport.</returns>
        public async Task<ConflictReport> AnalyzeAsync(IGame game, ModSelection selection, string collectionName, bool includeGame)
        {
            var allowedLanguages = languages.GetSelected();
            var modOrder = selection.Mods.Select(p => p.Name).ToList();
            var definitions = await patches.GetModObjectsAsync(game, selection.Mods, collectionName, PatchStateMode.ReadOnly, allowedLanguages);
            if (includeGame)
            {
                var versions = games.GetVersions(game)?.ToList();
                if (versions?.Count > 0)
                {
                    await gameIndex.IndexDefinitionsAsync(game, versions, definitions);
                    definitions = await gameIndex.LoadDefinitionsAsync(definitions, game, versions, allowedLanguages);
                }
            }

            var conflicts = await patches.FindConflictsAsync(definitions, modOrder, PatchStateMode.ReadOnly, allowedLanguages);
            var entries = await new ConflictReportBuilder(patches).BuildAsync(conflicts.Conflicts, modOrder);
            return new ConflictReport(game.Type, modOrder, selection.Unmatched, entries);
        }

        #endregion Methods
    }
}
