// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliConfigurationTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using IronyModManager.Shared;
using AwesomeAssertions;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class CliConfigurationTests.
    /// </summary>
    public class CliConfigurationTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method GetOptions_uses_given_storage_path.
        /// </summary>
        [Fact]
        public void GetOptions_uses_given_storage_path()
        {
            var root = Path.Combine(Path.GetTempPath(), "x");
            var path = new CliConfiguration(root).GetOptions().App.StoragePath;
            Path.GetFullPath(path, AppDomain.CurrentDomain.BaseDirectory).Should().Be(root);
        }

        /// <summary>
        /// Defines the test method GetOptions_storage_path_survives_irony_path_resolution.
        /// </summary>
        [Fact]
        public void GetOptions_storage_path_survives_irony_path_resolution()
        {
            var root = Path.Combine(Path.GetTempPath(), "IronyCLI-test");
            var configured = new CliConfiguration(root).GetOptions().App.StoragePath;

            // Same steps as DiskOperations.ResolveStoragePath, which drops the leading separator of rooted Unix paths
            var segments = configured.StandardizeDirectorySeparator().Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            var resolved = Path.GetFullPath(Path.Combine(segments), AppDomain.CurrentDomain.BaseDirectory);

            resolved.TrimEnd(Path.DirectorySeparatorChar).Should().Be(root.TrimEnd(Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// Defines the test method GetOptions_keeps_conflict_solver_in_memory.
        /// </summary>
        [Fact]
        public void GetOptions_keeps_conflict_solver_in_memory()
        {
            var options = new CliConfiguration("/tmp/x").GetOptions();
            options.ConflictSolver.UseHybridMemory.Should().BeFalse();
            options.ConflictSolver.UseDiskSearch.Should().BeFalse();
            options.ConflictSolver.CompressIndexedDefinitions.Should().BeTrue();
            options.Formatting.UseSystemCulture.Should().BeFalse();
        }

        #endregion Methods
    }
}
