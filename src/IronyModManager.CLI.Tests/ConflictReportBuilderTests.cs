// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ConflictReportBuilderTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeAssertions;
using IronyModManager.CLI.Conflicts;
using IronyModManager.Models.Common;
using IronyModManager.Services.Common;
using IronyModManager.Shared.Models;
using Moq;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class ConflictReportBuilderTests.
    /// </summary>
    public class ConflictReportBuilderTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method BuildAsync_reports_winner_and_priority_type.
        /// </summary>
        [Fact]
        public async Task BuildAsync_reports_winner_and_priority_type()
        {
            var stb = CreateDefinition("Sort Those Buildings", "common/buildings/00_capital_buildings.txt");
            var mmo = CreateDefinition("More Multiple Origins", "common/buildings/zz_MMO_00_capital_buildings.txt");
            var indexed = CreateIndex(stb, mmo);
            var patches = new Mock<IModPatchCollectionService>();
            IEnumerable<IDefinition> evaluated = null;
            patches.Setup(p => p.EvalDefinitionPriority(It.IsAny<IEnumerable<IDefinition>>()))
                .Callback<IEnumerable<IDefinition>>(p => evaluated = p.ToList())
                .Returns(CreatePriority(mmo, DefinitionPriorityType.LIOS, mmo, stb));

            var entries = await new ConflictReportBuilder(patches.Object).BuildAsync(indexed, ["More Multiple Origins", "Sort Those Buildings"]);

            entries.Should().ContainSingle();
            var entry = entries[0];
            entry.TypeAndId.Should().Be("k1");
            entry.Winner.Mod.Should().Be("More Multiple Origins");
            entry.Winner.File.Should().Be("common/buildings/zz_MMO_00_capital_buildings.txt");
            entry.PriorityType.Should().Be("LIOS");
            entry.Definitions.Should().HaveCount(2);
            evaluated.Select(p => p.ModName).Should().Equal("More Multiple Origins", "Sort Those Buildings");
        }

        /// <summary>
        /// Defines the test method BuildAsync_skips_single_mod_groups.
        /// </summary>
        [Fact]
        public async Task BuildAsync_skips_single_mod_groups()
        {
            var indexed = CreateIndex(CreateDefinition("A", "common/a/1.txt"), CreateDefinition("A", "common/a/2.txt"));
            var patches = new Mock<IModPatchCollectionService>();

            var entries = await new ConflictReportBuilder(patches.Object).BuildAsync(indexed, ["A"]);

            entries.Should().BeEmpty();
            patches.Verify(p => p.EvalDefinitionPriority(It.IsAny<IEnumerable<IDefinition>>()), Times.Never);
        }

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        /// <returns>IIndexedDefinitions.</returns>
        private static IIndexedDefinitions CreateIndex(params IDefinition[] definitions)
        {
            var indexed = new Mock<IIndexedDefinitions>();
            indexed.Setup(p => p.GetAllTypeAndIdKeysAsync()).ReturnsAsync(new[] { "k1" });
            indexed.Setup(p => p.GetByTypeAndIdAsync("k1")).ReturnsAsync(definitions);
            return indexed.Object;
        }

        /// <summary>
        /// Creates the definition.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <param name="file">The file.</param>
        /// <returns>IDefinition.</returns>
        private static IDefinition CreateDefinition(string mod, string file)
        {
            var definition = new Mock<IDefinition>();
            definition.SetupProperty(p => p.ModName, mod);
            definition.SetupProperty(p => p.File, file);
            definition.SetupProperty(p => p.Type, "common/buildings");
            definition.SetupProperty(p => p.Id, "building_clone_vats");
            definition.SetupGet(p => p.TypeAndId).Returns("k1");
            return definition.Object;
        }

        /// <summary>
        /// Creates the priority.
        /// </summary>
        /// <param name="winner">The winner.</param>
        /// <param name="type">The type.</param>
        /// <param name="order">The order.</param>
        /// <returns>IPriorityDefinitionResult.</returns>
        private static IPriorityDefinitionResult CreatePriority(IDefinition winner, DefinitionPriorityType type, params IDefinition[] order)
        {
            var result = new Mock<IPriorityDefinitionResult>();
            result.SetupProperty(p => p.Definition, winner);
            result.SetupProperty(p => p.PriorityType, type);
            result.SetupProperty(p => p.DefinitionOrder, order);
            return result.Object;
        }

        #endregion Methods
    }
}
