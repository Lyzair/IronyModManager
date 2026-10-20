// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="IronyDatabaseImporterTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using AwesomeAssertions;
using IronyModManager.CLI.Input;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class IronyDatabaseImporterTests.
    /// </summary>
    public class IronyDatabaseImporterTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method Import_copies_newest_database_and_leaves_source_untouched.
        /// </summary>
        [Fact]
        public void Import_copies_newest_database_and_leaves_source_untouched()
        {
            using var source = new TempStorage();
            using var target = new TempStorage();
            var guiDir = Directory.CreateDirectory(Path.Combine(source.Root, "IronyModManager")).FullName;
            var older = Path.Combine(guiDir, "Database_1.9.json");
            var newer = Path.Combine(guiDir, "Database_1.27.json");
            File.WriteAllText(older, "[\"old\"]");
            File.WriteAllText(newer, "[\"new\"]");
            var olderTime = File.GetLastWriteTimeUtc(older);
            var newerTime = File.GetLastWriteTimeUtc(newer);

            var copied = IronyDatabaseImporter.Import(source.Root, target.Root, "IronyModManager.CLI");

            copied.Should().Be(Path.Combine(target.Root, "IronyModManager.CLI", "Database_1.27.json"));
            File.ReadAllText(copied).Should().Be("[\"new\"]");
            File.ReadAllText(newer).Should().Be("[\"new\"]");
            File.ReadAllText(older).Should().Be("[\"old\"]");
            File.GetLastWriteTimeUtc(older).Should().Be(olderTime);
            File.GetLastWriteTimeUtc(newer).Should().Be(newerTime);
        }

        /// <summary>
        /// Defines the test method Import_without_database_throws_CliInputException.
        /// </summary>
        [Fact]
        public void Import_without_database_throws_CliInputException()
        {
            using var source = new TempStorage();
            using var target = new TempStorage();
            var act = () => IronyDatabaseImporter.Import(source.Root, target.Root, "IronyModManager.CLI");
            act.Should().Throw<CliInputException>().WithMessage($"*{source.Root}*");
        }

        #endregion Methods
    }
}
