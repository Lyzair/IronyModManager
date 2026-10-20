// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="PlaylistModMatcherTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using IronyModManager.CLI.Input;
using IronyModManager.Models.Common;
using Moq;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class PlaylistModMatcherTests.
    /// </summary>
    public class PlaylistModMatcherTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method Match_orders_by_position_and_skips_disabled.
        /// </summary>
        [Fact]
        public void Match_orders_by_position_and_skips_disabled()
        {
            var installed = new[] { CreateMod(3, "Three"), CreateMod(1, "One"), CreateMod(2, "Two") };
            var playlist = new LauncherPlaylist("stellaris", "test", new List<PlaylistEntry>
            {
                new("One", true, 0, "1", null),
                new("Two", false, 1, "2", null),
                new("Three", true, 2, "3", null)
            });

            var result = PlaylistModMatcher.Match(installed, playlist);

            result.Mods.Select(p => p.Name).Should().Equal("One", "Three");
            result.Unmatched.Should().BeEmpty();
        }

        /// <summary>
        /// Defines the test method Match_falls_back_to_name_when_remote_id_missing.
        /// </summary>
        [Fact]
        public void Match_falls_back_to_name_when_remote_id_missing()
        {
            var installed = new[] { CreateMod(null, "Local Patch") };
            var playlist = new LauncherPlaylist("stellaris", "test", new List<PlaylistEntry> { new("Local Patch", true, 0, "", null) });

            var result = PlaylistModMatcher.Match(installed, playlist);

            result.Mods.Select(p => p.Name).Should().Equal("Local Patch");
        }

        /// <summary>
        /// Defines the test method Match_reports_not_installed_entries.
        /// </summary>
        [Fact]
        public void Match_reports_not_installed_entries()
        {
            var installed = new[] { CreateMod(1, "One") };
            var playlist = new LauncherPlaylist("stellaris", "test", new List<PlaylistEntry>
            {
                new("Lord Commissar", true, 0, "1591095271", null),
                new("One", true, 1, "1", null)
            });

            var result = PlaylistModMatcher.Match(installed, playlist);

            result.Mods.Select(p => p.Name).Should().Equal("One");
            result.Unmatched.Select(p => p.SteamId).Should().Equal("1591095271");
        }

        /// <summary>
        /// Creates the mod.
        /// </summary>
        /// <param name="remoteId">The remote identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>IMod.</returns>
        private static IMod CreateMod(long? remoteId, string name)
        {
            var mod = new Mock<IMod>();
            mod.SetupProperty(p => p.RemoteId, remoteId);
            mod.SetupProperty(p => p.Name, name);
            return mod.Object;
        }

        #endregion Methods
    }
}
