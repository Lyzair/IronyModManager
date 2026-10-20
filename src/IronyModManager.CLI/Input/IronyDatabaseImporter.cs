// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="IronyDatabaseImporter.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;

namespace IronyModManager.CLI.Input
{
    /// <summary>
    /// Copies the GUI's database into the run's temporary storage so collections can be read without touching the GUI's files.
    /// </summary>
    public static class IronyDatabaseImporter
    {
        #region Fields

        /// <summary>
        /// The GUI storage segment
        /// </summary>
        private const string GuiSegment = "IronyModManager";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Imports the newest database of the GUI storage.
        /// </summary>
        /// <param name="ironyStorage">The GUI storage root, e.g. ~/.config/Mario.</param>
        /// <param name="tempRoot">The temporary storage root.</param>
        /// <param name="segment">The storage segment the CLI's database store reads from.</param>
        /// <returns>The path of the copy.</returns>
        /// <exception cref="CliInputException">No database was found.</exception>
        public static string Import(string ironyStorage, string tempRoot, string segment)
        {
            var source = Path.Combine(ironyStorage, GuiSegment);
            var newest = Directory.Exists(source)
                ? new DirectoryInfo(source).GetFiles("Database_*.json").OrderByDescending(ParseVersion).ThenByDescending(p => p.LastWriteTimeUtc).FirstOrDefault()
                : null;
            if (newest == null)
            {
                throw new CliInputException($"No Irony database found in '{source}'");
            }

            var targetDirectory = Directory.CreateDirectory(Path.Combine(tempRoot, segment)).FullName;
            var target = Path.Combine(targetDirectory, newest.Name);
            newest.CopyTo(target, true);
            return target;
        }

        /// <summary>
        /// Parses the version from a file name like Database_1.27.json.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Version.</returns>
        private static Version ParseVersion(FileInfo file)
        {
            var name = Path.GetFileNameWithoutExtension(file.Name);
            return Version.TryParse(name[(name.IndexOf('_') + 1)..], out var version) ? version : new Version(0, 0);
        }

        #endregion Methods
    }
}
