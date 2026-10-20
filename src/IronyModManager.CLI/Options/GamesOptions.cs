// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="GamesOptions.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using CommandLine;

namespace IronyModManager.CLI.Options
{
    /// <summary>
    /// Options of the games verb.
    /// </summary>
    [Verb("games", HelpText = "List the games Irony knows with the user and workshop folders it would use.")]
    public class GamesOptions
    {
    }
}
