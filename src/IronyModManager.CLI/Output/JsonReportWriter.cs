// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="JsonReportWriter.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using IronyModManager.CLI.Conflicts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IronyModManager.CLI.Output
{
    /// <summary>
    /// Writes a conflict report as JSON.
    /// </summary>
    public static class JsonReportWriter
    {
        #region Methods

        /// <summary>
        /// Writes the specified report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        public static void Write(ConflictReport report, TextWriter writer)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            writer.WriteLine(JsonConvert.SerializeObject(report, settings));
        }

        #endregion Methods
    }
}
