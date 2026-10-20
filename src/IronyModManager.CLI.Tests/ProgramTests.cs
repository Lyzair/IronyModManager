// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ProgramTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using AwesomeAssertions;
using IronyModManager.Shared.Configuration;
using Moq;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class ProgramTests.
    /// </summary>
    public class ProgramTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method EnsureCliConfiguration_rejects_foreign_configuration.
        /// </summary>
        [Fact]
        public void EnsureCliConfiguration_rejects_foreign_configuration()
        {
            var act = () => Program.EnsureCliConfiguration(new Mock<IDomainConfiguration>().Object);
            act.Should().Throw<InvalidOperationException>().WithMessage("*own folder*");
        }

        /// <summary>
        /// Defines the test method EnsureCliConfiguration_accepts_cli_configuration.
        /// </summary>
        [Fact]
        public void EnsureCliConfiguration_accepts_cli_configuration()
        {
            var act = () => Program.EnsureCliConfiguration(new CliConfiguration("x"));
            act.Should().NotThrow();
        }

        #endregion Methods
    }
}
