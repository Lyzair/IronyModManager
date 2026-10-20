// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliLogger.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using IronyModManager.Shared;
using NLog;

namespace IronyModManager.CLI.Logging
{
    /// <summary>
    /// Logger of the CLI, routed by NLog to stderr only.
    /// Implements the <see cref="IronyModManager.Shared.ILogger" />
    /// </summary>
    /// <seealso cref="IronyModManager.Shared.ILogger" />
    public class CliLogger : Shared.ILogger
    {
        #region Fields

        /// <summary>
        /// The log
        /// </summary>
        private static readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Errors the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public void Error(Exception ex, string message = Constants.EmptyParam)
        {
            if (ex != null)
            {
                log.Error(ex, message);
            }
        }

        /// <summary>
        /// Fatals the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public void Fatal(Exception ex, string message = Constants.EmptyParam)
        {
            if (ex != null)
            {
                log.Fatal(ex, message);
            }
        }

        /// <summary>
        /// Gets the last fatal exception message. The CLI keeps no log files, so there is none.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetLastFatalExceptionMessage()
        {
            return string.Empty;
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                log.Trace(message);
            }
        }

        #endregion Methods
    }
}
