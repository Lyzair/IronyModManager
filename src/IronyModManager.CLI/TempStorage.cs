// ***********************************************************************
// Assembly         : IronyModManager.CLI
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="TempStorage.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;

namespace IronyModManager.CLI
{
    /// <summary>
    /// Throwaway storage folder for a single CLI run.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class TempStorage : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TempStorage" /> class.
        /// </summary>
        public TempStorage()
        {
            Root = Path.Combine(Path.GetTempPath(), $"IronyCLI-{Guid.NewGuid():N}");
            Directory.CreateDirectory(Root);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>The root.</value>
        public string Root { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deletes the storage folder.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (Directory.Exists(Root))
                {
                    Directory.Delete(Root, true);
                }
            }
            catch
            {
                // Cleanup is best effort, a failed delete must not mask the run's result
            }
        }

        #endregion Methods
    }
}
