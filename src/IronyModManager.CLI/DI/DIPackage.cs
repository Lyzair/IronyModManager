// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="DIPackage.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using IronyModManager.CLI.Logging;
using IronyModManager.Localization.ResourceProviders;
using IronyModManager.Shared.Cache;
using IronyModManager.Shared.Configuration;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace IronyModManager.CLI.DI
{
    /// <summary>
    /// Registers what the GUI project normally provides to the services.
    /// Implements the <see cref="SimpleInjector.Packaging.IPackage" />
    /// </summary>
    /// <seealso cref="SimpleInjector.Packaging.IPackage" />
    public class DIPackage : IPackage
    {
        #region Methods

        /// <summary>
        /// Registers the set of services in the specified <paramref name="container" />.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            container.Register<IDomainConfiguration>(() => new CliConfiguration(CliConfiguration.StorageRoot), Lifestyle.Singleton);
            container.Register<Shared.ILogger, CliLogger>(Lifestyle.Singleton);
            container.Register<ICache, Cache>(Lifestyle.Singleton);
            container.Collection.Register<ILocalizationResourceProvider>(typeof(CliLocalizationResourceProvider));
            container.Register<IDefaultLocalizationResourceProvider, CliLocalizationResourceProvider>();
        }

        #endregion Methods
    }
}
