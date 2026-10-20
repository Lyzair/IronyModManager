// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CommonOptions.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using CommandLine;

namespace IronyModManager.CLI.Options
{
    /// <summary>
    /// Options shared by all verbs.
    /// </summary>
    public abstract class CommonOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the game.
        /// </summary>
        /// <value>The game.</value>
        [Option("game", Default = "Stellaris", HelpText = "Game type as listed by the games verb.")]
        public string Game { get; set; }

        #endregion Properties
    }
}
