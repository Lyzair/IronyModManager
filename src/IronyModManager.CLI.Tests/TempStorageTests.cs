// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="TempStorageTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using AwesomeAssertions;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class TempStorageTests.
    /// </summary>
    public class TempStorageTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method Root_exists_under_temp_and_is_removed_on_dispose.
        /// </summary>
        [Fact]
        public void Root_exists_under_temp_and_is_removed_on_dispose()
        {
            var storage = new TempStorage();
            var root = storage.Root;
            root.Should().StartWith(Path.GetTempPath());
            Path.GetFileName(root).Should().StartWith("IronyCLI-");
            Directory.Exists(root).Should().BeTrue();
            Directory.CreateDirectory(Path.Combine(root, "nested"));
            File.WriteAllText(Path.Combine(root, "nested", "file.json"), "{}");

            storage.Dispose();

            Directory.Exists(root).Should().BeFalse();
        }

        /// <summary>
        /// Defines the test method Dispose_twice_does_not_throw.
        /// </summary>
        [Fact]
        public void Dispose_twice_does_not_throw()
        {
            var storage = new TempStorage();
            storage.Dispose();
            var act = () => storage.Dispose();
            act.Should().NotThrow();
        }

        #endregion Methods
    }
}
