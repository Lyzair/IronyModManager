// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="TextReportWriter.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using IronyModManager.CLI.Conflicts;

namespace IronyModManager.CLI.Output
{
    /// <summary>
    /// Writes a conflict report for humans.
    /// </summary>
    public static class TextReportWriter
    {
        #region Methods

        /// <summary>
        /// Writes the specified report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        public static void Write(ConflictReport report, TextWriter writer)
        {
            writer.WriteLine($"{report.Game}: {report.Mods.Count} mods, {report.Conflicts.Count} conflicts");
            if (report.Unmatched.Count > 0)
            {
                writer.WriteLine($"Not installed ({report.Unmatched.Count}):");
                foreach (var entry in report.Unmatched)
                {
                    writer.WriteLine($"  {entry.Position}: {entry.DisplayName} ({entry.SteamId})");
                }
            }

            foreach (var conflict in report.Conflicts)
            {
                writer.WriteLine();
                writer.WriteLine($"{conflict.Type} {conflict.Id} [{conflict.PriorityType}] winner: {Format(conflict.Winner)}");
                foreach (var definition in conflict.Definitions)
                {
                    if (definition != conflict.Winner)
                    {
                        writer.WriteLine($"    {Format(definition)}");
                    }
                }
            }
        }

        /// <summary>
        /// Formats the specified definition.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns>System.String.</returns>
        private static string Format(DefinitionRef definition)
        {
            if (definition == null)
            {
                return "unknown";
            }

            return definition.IsFromGame ? $"{definition.Mod} (game: {definition.File})" : $"{definition.Mod} ({definition.File})";
        }

        #endregion Methods
    }
}
