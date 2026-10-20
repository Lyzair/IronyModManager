// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ConflictReportBuilder.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronyModManager.Services.Common;
using IronyModManager.Shared.Models;

namespace IronyModManager.CLI.Conflicts
{
    /// <summary>
    /// Turns Irony's conflict index into report entries.
    /// </summary>
    /// <param name="patches">The patch collection service.</param>
    public class ConflictReportBuilder(IModPatchCollectionService patches)
    {
        #region Methods

        /// <summary>
        /// Builds the entries, one per key that more than one mod defines.
        /// </summary>
        /// <param name="conflicts">The conflicts.</param>
        /// <param name="modOrder">The mod order.</param>
        /// <returns>The entries ordered by key.</returns>
        public async Task<IReadOnlyList<ConflictEntry>> BuildAsync(IIndexedDefinitions conflicts, IList<string> modOrder)
        {
            var entries = new List<ConflictEntry>();
            foreach (var key in (await conflicts.GetAllTypeAndIdKeysAsync()).OrderBy(p => p, StringComparer.Ordinal))
            {
                // Same ordering the GUI's compare view uses before evaluating, game definitions (index -1) come first
                var definitions = (await conflicts.GetByTypeAndIdAsync(key)).OrderBy(p => modOrder.IndexOf(p.ModName)).ToList();
                if (definitions.Select(p => p.ModName).Distinct().Count() < 2)
                {
                    continue;
                }

                var priority = patches.EvalDefinitionPriority(definitions);
                var first = definitions[0];
                entries.Add(new ConflictEntry(key, first.Type, first.Id, first.ValueType.ToString(), priority?.PriorityType.ToString() ?? "None",
                    ToRef(priority?.Definition), (priority?.DefinitionOrder ?? definitions).Select(ToRef).ToList()));
            }

            return entries;
        }

        /// <summary>
        /// Converts to a reference.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns>DefinitionRef.</returns>
        private static DefinitionRef ToRef(IDefinition definition)
        {
            return definition == null ? null : new DefinitionRef(definition.ModName, definition.File, definition.IsFromGame);
        }

        #endregion Methods
    }
}
