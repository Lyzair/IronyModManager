// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliConfiguration.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using IronyModManager.Shared.Configuration;

namespace IronyModManager.CLI
{
    /// <summary>
    /// Domain configuration of the CLI. Built in code so the CLI ships and reads no configuration files.
    /// Implements the <see cref="IronyModManager.Shared.Configuration.IDomainConfiguration" />
    /// </summary>
    /// <seealso cref="IronyModManager.Shared.Configuration.IDomainConfiguration" />
    public class CliConfiguration : IDomainConfiguration
    {
        #region Fields

        /// <summary>
        /// The storage path
        /// </summary>
        private readonly string storagePath;

        /// <summary>
        /// The options
        /// </summary>
        private DomainConfigurationOptions options;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CliConfiguration" /> class.
        /// </summary>
        /// <param name="storagePath">The storage path.</param>
        public CliConfiguration(string storagePath)
        {
            this.storagePath = storagePath;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the storage root of the current run, used when the DI container creates the configuration.
        /// </summary>
        /// <value>The storage root.</value>
        public static string StorageRoot { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <returns>DomainConfigurationOptions.</returns>
        public DomainConfigurationOptions GetOptions()
        {
            if (options == null)
            {
                var result = new DomainConfigurationOptions();
                result.App.StoragePath = storagePath;
                result.Formatting.UseSystemCulture = false;
                result.Steam.UseGameHandler = true;
                result.Steam.GameHandlerPath = "IronyModManager.GameHandler.exe";
                // Disk backed stores would write into the storage, keep definitions in memory instead
                result.ConflictSolver.UseHybridMemory = false;
                result.ConflictSolver.UseDiskSearch = false;
                result.ConflictSolver.CompressIndexedDefinitions = true;
                options = result;
            }

            return options;
        }

        #endregion Methods
    }
}
