// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ConflictsOptionsTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using AwesomeAssertions;
using IronyModManager.CLI.Options;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class ConflictsOptionsTests.
    /// </summary>
    public class ConflictsOptionsTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method ResolvePaths_makes_relative_paths_absolute_against_working_directory.
        /// </summary>
        [Fact]
        public void ResolvePaths_makes_relative_paths_absolute_against_working_directory()
        {
            var cwd = Path.Combine(Path.GetTempPath(), "work");
            var options = new ConflictsOptions { Playlist = "w40k.json", IronyStorage = Path.Combine("cfg", "Mario") };

            options.ResolvePaths(cwd);

            options.Playlist.Should().Be(Path.Combine(cwd, "w40k.json"));
            options.IronyStorage.Should().Be(Path.Combine(cwd, "cfg", "Mario"));
        }

        /// <summary>
        /// Defines the test method ResolvePaths_keeps_absolute_and_missing_paths.
        /// </summary>
        [Fact]
        public void ResolvePaths_keeps_absolute_and_missing_paths()
        {
            var storage = Path.Combine(Path.GetTempPath(), "Mario");
            var options = new ConflictsOptions { Playlist = null, IronyStorage = storage };

            options.ResolvePaths(Path.Combine(Path.GetTempPath(), "work"));

            options.Playlist.Should().BeNull();
            options.IronyStorage.Should().Be(storage);
        }

        #endregion Methods
    }
}
