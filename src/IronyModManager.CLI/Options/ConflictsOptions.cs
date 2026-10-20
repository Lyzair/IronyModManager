// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ConflictsOptions.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using CommandLine;

namespace IronyModManager.CLI.Options
{
    /// <summary>
    /// Options of the conflicts verb.
    /// Implements the <see cref="IronyModManager.CLI.Options.CommonOptions" />
    /// </summary>
    /// <seealso cref="IronyModManager.CLI.Options.CommonOptions" />
    [Verb("conflicts", HelpText = "Report every conflicting definition with its winner and the reason.")]
    public class ConflictsOptions : CommonOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        /// <value>The collection.</value>
        [Option("collection", HelpText = "Name of an Irony GUI collection, read from a copy of the GUI's database.")]
        public string Collection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the base game's definitions take part.
        /// </summary>
        /// <value><c>true</c> if the game is included; otherwise, <c>false</c>.</value>
        [Option("include-game", HelpText = "Include the base game's definitions (indexes the game on every run).")]
        public bool IncludeGame { get; set; }

        /// <summary>
        /// Gets or sets the Irony GUI storage folder.
        /// </summary>
        /// <value>The Irony storage.</value>
        [Option("irony-storage", HelpText = "Irony GUI storage folder, only read with --collection. Default: <ApplicationData>/Mario.")]
        public string IronyStorage { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mario");

        /// <summary>
        /// Gets or sets a value indicating whether to write JSON.
        /// </summary>
        /// <value><c>true</c> if JSON; otherwise, <c>false</c>.</value>
        [Option("json", HelpText = "Write the report as JSON.")]
        public bool Json { get; set; }

        /// <summary>
        /// Gets or sets the playlist.
        /// </summary>
        /// <value>The playlist.</value>
        [Option("playlist", HelpText = "Paradox Launcher playlist export (JSON).")]
        public string Playlist { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Makes the path options absolute, the CLI changes its working directory to its own folder after parsing.
        /// </summary>
        /// <param name="workingDirectory">The user's working directory.</param>
        public void ResolvePaths(string workingDirectory)
        {
            Playlist = Resolve(Playlist, workingDirectory);
            IronyStorage = Resolve(IronyStorage, workingDirectory);
        }

        /// <summary>
        /// Resolves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="workingDirectory">The working directory.</param>
        /// <returns>System.String.</returns>
        private static string Resolve(string path, string workingDirectory)
        {
            return string.IsNullOrWhiteSpace(path) ? path : Path.GetFullPath(path, workingDirectory);
        }

        #endregion Methods
    }
}
