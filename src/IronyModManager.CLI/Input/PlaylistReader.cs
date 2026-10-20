// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="PlaylistReader.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IronyModManager.CLI.Input
{
    /// <summary>
    /// Reads Paradox Launcher playlist exports.
    /// </summary>
    public static class PlaylistReader
    {
        #region Methods

        /// <summary>
        /// Reads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>LauncherPlaylist.</returns>
        /// <exception cref="CliInputException">The file is missing or not a playlist.</exception>
        public static LauncherPlaylist Read(string path)
        {
            JObject json;
            try
            {
                json = JObject.Parse(File.ReadAllText(path));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
            {
                throw new CliInputException($"Cannot read playlist '{path}': {ex.Message}", ex);
            }

            if (json["mods"] is not JArray mods)
            {
                throw new CliInputException($"Playlist '{path}' has no mods array");
            }

            var entries = mods.OfType<JObject>().Select(p => new PlaylistEntry(
                (string)p["displayName"],
                (bool?)p["enabled"] ?? true,
                (int?)p["position"] ?? int.MaxValue,
                (string)p["steamId"],
                (string)p["pdxId"])).OrderBy(p => p.Position).ToList();
            return new LauncherPlaylist((string)json["game"], (string)json["name"], entries);
        }

        #endregion Methods
    }
}
