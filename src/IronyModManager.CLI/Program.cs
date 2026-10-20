// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="Program.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using IronyModManager.CLI.Conflicts;
using IronyModManager.CLI.Input;
using IronyModManager.CLI.Options;
using IronyModManager.DI;
using IronyModManager.Localization;
using IronyModManager.Services.Common;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace IronyModManager.CLI
{
    /// <summary>
    /// Entry point of the CLI.
    /// </summary>
    public static class Program
    {
        #region Methods

        /// <summary>
        /// Initializes the services against the given storage root.
        /// </summary>
        /// <param name="storageRoot">The storage root.</param>
        public static void InitServices(string storageRoot)
        {
            CliConfiguration.StorageRoot = storageRoot;
            CurrentLocale.SetCurrent(Shared.Constants.DefaultAppCulture);
            InitLogging();
            Bootstrap.Setup(new DIOptions { Container = new SimpleInjector.Container(), PluginPathAndName = Shared.Constants.PluginsPathAndName });
            Bootstrap.PostStartup();
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>System.Int32.</returns>
        public static int Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var result = CommandLine.Parser.Default.ParseArguments(args, typeof(GamesOptions), typeof(ConflictsOptions));
            if (result.Tag == ParserResultType.NotParsed)
            {
                return 2;
            }

            using var storage = new TempStorage();
            try
            {
                if (result.Value is ConflictsOptions { Collection: { Length: > 0 } } collectionOptions)
                {
                    IronyDatabaseImporter.Import(collectionOptions.IronyStorage, storage.Root, GetStorageSegment());
                }

                InitServices(storage.Root);
                return result.Value switch
                {
                    GamesOptions => RunGames(Console.Out),
                    ConflictsOptions conflictsOptions => CreateRunner().RunConflictsAsync(conflictsOptions, Console.Out, Console.Error).GetAwaiter().GetResult(),
                    _ => 2
                };
            }
            catch (CliInputException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 2;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex}");
                return 1;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Creates the runner from the services.
        /// </summary>
        /// <returns>CliRunner.</returns>
        private static CliRunner CreateRunner()
        {
            var games = DIResolver.Get<IGameService>();
            var patches = DIResolver.Get<IModPatchCollectionService>();
            var analyzer = new ConflictAnalyzer(games, DIResolver.Get<IGameLanguageService>(), patches, DIResolver.Get<IGameIndexService>());
            return new CliRunner(games, DIResolver.Get<IModService>(), DIResolver.Get<IModCollectionService>(), analyzer);
        }

        /// <summary>
        /// Gets the folder below the storage root that Irony's database store uses, derived from the entry assembly's title like JsonStore does.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetStorageSegment()
        {
            return Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
        }

        /// <summary>
        /// Configures NLog to write warnings and errors to stderr, stdout stays reserved for command output.
        /// </summary>
        private static void InitLogging()
        {
            var config = new LoggingConfiguration();
            var stderr = new ConsoleTarget("stderr") { StdErr = true, Layout = "${level:uppercase=true}: ${message} ${exception}" };
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, stderr);
            LogManager.Configuration = config;
        }

        /// <summary>
        /// Runs the games verb.
        /// </summary>
        /// <param name="stdout">The stdout.</param>
        /// <returns>System.Int32.</returns>
        private static int RunGames(TextWriter stdout)
        {
            foreach (var game in DIResolver.Get<IGameService>().Get())
            {
                stdout.WriteLine($"{game.Type}\t{game.UserDirectory}\t{game.WorkshopDirectory?.FirstOrDefault()}");
            }

            return 0;
        }

        #endregion Methods
    }
}
