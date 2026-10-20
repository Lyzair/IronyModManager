// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="ReportWriterTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using AwesomeAssertions;
using IronyModManager.CLI.Conflicts;
using IronyModManager.CLI.Input;
using IronyModManager.CLI.Output;
using Newtonsoft.Json.Linq;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class ReportWriterTests.
    /// </summary>
    public class ReportWriterTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method Json_roundtrips_report.
        /// </summary>
        [Fact]
        public void Json_roundtrips_report()
        {
            var writer = new StringWriter();

            JsonReportWriter.Write(CreateReport(), writer);

            var json = JObject.Parse(writer.ToString());
            ((string)json["conflicts"]![0]!["winner"]!["mod"]).Should().Be("More Multiple Origins");
            ((string)json["conflicts"]![0]!["priorityType"]).Should().Be("LIOS");
            ((string)json["unmatched"]![0]!["steamId"]).Should().Be("1591095271");
            ((string)json["game"]).Should().Be("Stellaris");
        }

        /// <summary>
        /// Defines the test method Text_lists_winner_line.
        /// </summary>
        [Fact]
        public void Text_lists_winner_line()
        {
            var writer = new StringWriter();

            TextReportWriter.Write(CreateReport(), writer);

            var text = writer.ToString();
            text.Should().Contain("building_clone_vats [LIOS] winner: More Multiple Origins (common/buildings/zz_MMO_00_capital_buildings.txt)");
            text.Should().Contain("Sort Those Buildings (common/buildings/00_capital_buildings.txt)");
            text.Should().Contain("1591095271");
        }

        /// <summary>
        /// Creates the report.
        /// </summary>
        /// <returns>ConflictReport.</returns>
        private static ConflictReport CreateReport()
        {
            var mmo = new DefinitionRef("More Multiple Origins", "common/buildings/zz_MMO_00_capital_buildings.txt", false);
            var stb = new DefinitionRef("Sort Those Buildings", "common/buildings/00_capital_buildings.txt", false);
            return new ConflictReport("Stellaris", ["Sort Those Buildings", "More Multiple Origins"],
                [new PlaylistEntry("Lord Commissar", true, 3, "1591095271", null)],
                [new ConflictEntry("common/buildings-building_clone_vats", "common/buildings", "building_clone_vats", "Object", "LIOS", mmo, [mmo, stb])]);
        }

        #endregion Methods
    }
}
