// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CliLocalizationResourceProvider.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronyModManager.Localization.ResourceProviders;

namespace IronyModManager.CLI
{
    /// <summary>
    /// Reads the GUI's localization files copied next to the CLI.
    /// Implements the <see cref="IronyModManager.Localization.ResourceProviders.BaseLocalizationResourceProvider" />
    /// Implements the <see cref="IronyModManager.Localization.ResourceProviders.IDefaultLocalizationResourceProvider" />
    /// </summary>
    /// <seealso cref="IronyModManager.Localization.ResourceProviders.BaseLocalizationResourceProvider" />
    /// <seealso cref="IronyModManager.Localization.ResourceProviders.IDefaultLocalizationResourceProvider" />
    public class CliLocalizationResourceProvider : BaseLocalizationResourceProvider, IDefaultLocalizationResourceProvider
    {
        #region Properties

        /// <summary>
        /// Gets the root path.
        /// </summary>
        /// <value>The root path.</value>
        public override string RootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization");

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the available locales.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public virtual IEnumerable<string> GetAvailableLocales()
        {
            return new DirectoryInfo(RootPath).GetFiles($"*{Shared.Constants.JsonExtension}", SearchOption.TopDirectoryOnly).Select(p => Path.GetFileNameWithoutExtension(p.Name));
        }

        #endregion Methods
    }
}
