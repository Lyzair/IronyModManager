// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="IConflictAnalyzer.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using IronyModManager.CLI.Input;
using IronyModManager.Models.Common;

namespace IronyModManager.CLI.Conflicts
{
    /// <summary>
    /// Runs Irony's conflict solver on a mod selection.
    /// </summary>
    public interface IConflictAnalyzer
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
        Task<ConflictReport> AnalyzeAsync(IGame game, ModSelection selection, string collectionName, bool includeGame);

        #endregion Methods
    }
}
