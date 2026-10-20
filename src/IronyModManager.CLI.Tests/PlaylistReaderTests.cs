// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="PlaylistReaderTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using AwesomeAssertions;
using IronyModManager.CLI.Input;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class PlaylistReaderTests.
    /// </summary>
    public class PlaylistReaderTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method Read_parses_v3_playlist_sorted_by_position.
        /// </summary>
        [Fact]
        public void Read_parses_v3_playlist_sorted_by_position()
        {
            var path = WriteTemp("""
                {"game":"stellaris","name":"w40k","mods":[
                  {"displayName":"C","enabled":true,"position":2,"steamId":"3"},
                  {"displayName":"A","enabled":true,"position":0,"steamId":"1","pdxId":"p1"},
                  {"displayName":"B","enabled":false,"position":1,"steamId":"2"}]}
                """);

            var playlist = PlaylistReader.Read(path);

            playlist.Name.Should().Be("w40k");
            playlist.Game.Should().Be("stellaris");
            playlist.Mods.Select(p => p.Position).Should().Equal(0, 1, 2);
            playlist.Mods[0].Should().Be(new PlaylistEntry("A", true, 0, "1", "p1"));
            playlist.Mods[1].Enabled.Should().BeFalse();
        }

        /// <summary>
        /// Defines the test method Read_missing_file_throws_CliInputException.
        /// </summary>
        [Fact]
        public void Read_missing_file_throws_CliInputException()
        {
            var act = () => PlaylistReader.Read(Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json"));
            act.Should().Throw<CliInputException>();
        }

        /// <summary>
        /// Defines the test method Read_invalid_json_throws_CliInputException.
        /// </summary>
        [Fact]
        public void Read_invalid_json_throws_CliInputException()
        {
            var act = () => PlaylistReader.Read(WriteTemp("{ not json"));
            act.Should().Throw<CliInputException>();
        }

        /// <summary>
        /// Writes the temporary.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>System.String.</returns>
        private static string WriteTemp(string content)
        {
            var path = Path.Combine(Path.GetTempPath(), $"irony-cli-test-{Guid.NewGuid():N}.json");
            File.WriteAllText(path, content);
            return path;
        }

        #endregion Methods
    }
}
