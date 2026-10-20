// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliRunnerTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Threading.Tasks;
using AwesomeAssertions;
using IronyModManager.CLI.Conflicts;
using IronyModManager.CLI.Input;
using IronyModManager.CLI.Options;
using IronyModManager.Models.Common;
using IronyModManager.Services.Common;
using IronyModManager.Services.Common.Exceptions;
using Moq;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class CliRunnerTests.
    /// </summary>
    public class CliRunnerTests
    {
        #region Fields

        /// <summary>
        /// The playlist path
        /// </summary>
        private static readonly string playlistPath = WritePlaylist();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Defines the test method RunConflicts_unknown_game_returns_2.
        /// </summary>
        [Fact]
        public async Task RunConflicts_unknown_game_returns_2()
        {
            var (runner, _) = CreateRunner();
            var stdout = new StringWriter();
            var stderr = new StringWriter();

            var code = await runner.RunConflictsAsync(new ConflictsOptions { Game = "Hoi4", Playlist = playlistPath }, stdout, stderr);

            code.Should().Be(2);
            stderr.ToString().Should().Contain("Hoi4");
            stdout.ToString().Should().BeEmpty();
        }

        /// <summary>
        /// Defines the test method RunConflicts_too_large_mod_returns_3.
        /// </summary>
        [Fact]
        public async Task RunConflicts_too_large_mod_returns_3()
        {
            var (runner, analyzer) = CreateRunner();
            analyzer.Setup(p => p.AnalyzeAsync(It.IsAny<IGame>(), It.IsAny<ModSelection>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new ModTooLargeException("too large"));
            var stderr = new StringWriter();

            var code = await runner.RunConflictsAsync(new ConflictsOptions { Game = "Stellaris", Playlist = playlistPath }, new StringWriter(), stderr);

            code.Should().Be(3);
            stderr.ToString().Should().Contain("too large");
        }

        /// <summary>
        /// Defines the test method RunConflicts_writes_json_report_to_stdout.
        /// </summary>
        [Fact]
        public async Task RunConflicts_writes_json_report_to_stdout()
        {
            var (runner, analyzer) = CreateRunner();
            analyzer.Setup(p => p.AnalyzeAsync(It.IsAny<IGame>(), It.IsAny<ModSelection>(), "IronyCLI-t", false))
                .ReturnsAsync(new ConflictReport("Stellaris", [], [], []));
            var stdout = new StringWriter();

            var code = await runner.RunConflictsAsync(new ConflictsOptions { Game = "Stellaris", Playlist = playlistPath, Json = true }, stdout, new StringWriter());

            code.Should().Be(0);
            ((string)Newtonsoft.Json.Linq.JObject.Parse(stdout.ToString())["game"]).Should().Be("Stellaris");
        }

        /// <summary>
        /// Defines the test method RunConflicts_requires_exactly_one_input.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        /// <param name="collection">The collection.</param>
        [Theory]
        [InlineData(null, null)]
        [InlineData("x.json", "w40k")]
        public async Task RunConflicts_requires_exactly_one_input(string playlist, string collection)
        {
            var (runner, _) = CreateRunner();

            var code = await runner.RunConflictsAsync(new ConflictsOptions { Game = "Stellaris", Playlist = playlist, Collection = collection }, new StringWriter(), new StringWriter());

            code.Should().Be(2);
        }

        /// <summary>
        /// Creates the runner.
        /// </summary>
        /// <returns>A runner and its analyzer mock.</returns>
        private static (CliRunner, Mock<IConflictAnalyzer>) CreateRunner()
        {
            var stellaris = new Mock<IGame>();
            stellaris.SetupProperty(p => p.Type, "Stellaris");
            stellaris.SetupProperty(p => p.IsSelected, true);
            var games = new Mock<IGameService>();
            games.Setup(p => p.Get()).Returns(new[] { stellaris.Object });
            games.Setup(p => p.GetSelected()).Returns(stellaris.Object);
            var mods = new Mock<IModService>();
            mods.Setup(p => p.GetInstalledModsAsync(It.IsAny<IGame>())).ReturnsAsync(Array.Empty<IMod>());
            var analyzer = new Mock<IConflictAnalyzer>();
            return (new CliRunner(games.Object, mods.Object, new Mock<IModCollectionService>().Object, analyzer.Object), analyzer);
        }

        /// <summary>
        /// Writes the playlist.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string WritePlaylist()
        {
            var path = Path.Combine(Path.GetTempPath(), $"irony-cli-test-{Guid.NewGuid():N}.json");
            File.WriteAllText(path, """{"game":"stellaris","name":"t","mods":[]}""");
            return path;
        }

        #endregion Methods
    }
}
