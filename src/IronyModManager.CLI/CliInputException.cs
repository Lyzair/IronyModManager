// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliInputException.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace IronyModManager.CLI
{
    /// <summary>
    /// Invalid user input, reported with exit code 2.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CliInputException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CliInputException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CliInputException(string message, Exception innerException = null) : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}
