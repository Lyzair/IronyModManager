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
            var config = new CliConfiguration("/tmp/x");
            config.GetOptions().App.StoragePath.Should().Be("/tmp/x");
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
