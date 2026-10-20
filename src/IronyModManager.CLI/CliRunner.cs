// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliRunner.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IronyModManager.CLI.Conflicts;
using IronyModManager.CLI.Input;
using IronyModManager.CLI.Options;
using IronyModManager.Services.Common;
using IronyModManager.Services.Common.Exceptions;

namespace IronyModManager.CLI
{
    /// <summary>
    /// Runs the CLI verbs that need the services.
    /// </summary>
    /// <param name="games">The game service.</param>
    /// <param name="mods">The mod service.</param>
    /// <param name="collections">The collection service.</param>
    /// <param name="analyzer">The analyzer.</param>
    public class CliRunner(IGameService games, IModService mods, IModCollectionService collections, IConflictAnalyzer analyzer)
    {
        #region Methods

        /// <summary>
        /// Runs the conflicts verb.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="stdout">The stdout.</param>
        /// <param name="stderr">The stderr.</param>
        /// <returns>The exit code: 0 success, 1 unexpected error, 2 bad input, 3 a mod is too large to parse.</returns>
        public async Task<int> RunConflictsAsync(ConflictsOptions options, TextWriter stdout, TextWriter stderr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(options.Playlist) == string.IsNullOrWhiteSpace(options.Collection))
                {
                    throw new CliInputException("Pass exactly one of --playlist or --collection");
                }

                var allGames = games.Get().ToList();
                var game = allGames.FirstOrDefault(p => p.Type.Equals(options.Game, StringComparison.OrdinalIgnoreCase))
                           ?? throw new CliInputException($"Unknown game '{options.Game}', known: {string.Join(", ", allGames.Select(p => p.Type))}");
                if (!game.IsSelected)
                {
                    games.SetSelected(allGames, game);
                }

                var installed = await mods.GetInstalledModsAsync(game);
                ModSelection selection;
                string collectionName;
                if (!string.IsNullOrWhiteSpace(options.Playlist))
                {
                    var playlist = PlaylistReader.Read(options.Playlist);
                    selection = PlaylistModMatcher.Match(installed, playlist);
                    collectionName = $"IronyCLI-{playlist.Name}";
                }
                else
                {
                    selection = new CollectionModSource(collections, mods).Resolve(options.Collection, installed);
                    collectionName = options.Collection;
                }

                await analyzer.AnalyzeAsync(game, selection, collectionName, options.IncludeGame);
                return 0;
            }
            catch (CliInputException ex)
            {
                await stderr.WriteLineAsync(ex.Message);
                return 2;
            }
            catch (ModTooLargeException ex)
            {
                await stderr.WriteLineAsync($"A mod is too large to parse: {ex.Message}");
                return 3;
            }
            catch (Exception ex)
            {
                await stderr.WriteLineAsync($"Unexpected error: {ex}");
                return 1;
            }
        }

        #endregion Methods
    }
}
