// ***********************************************************************
// Assembly         : IronyModManager.CLI.Tests
// Author           : Lyzair
// Created          : 10-20-2026
//
// Last Modified By : Lyzair
// Last Modified On : 10-20-2026
// ***********************************************************************
// <copyright file="CollectionModSourceTests.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using IronyModManager.CLI.Input;
using IronyModManager.Models.Common;
using IronyModManager.Services.Common;
using Moq;
using Xunit;

namespace IronyModManager.CLI.Tests
{
    /// <summary>
    /// Class CollectionModSourceTests.
    /// </summary>
    public class CollectionModSourceTests
    {
        #region Methods

        /// <summary>
        /// Defines the test method Resolve_returns_collection_mods_in_collection_order.
        /// </summary>
        [Fact]
        public void Resolve_returns_collection_mods_in_collection_order()
        {
            var collection = new Mock<IModCollection>().Object;
            var installed = new[] { CreateMod("A", false), CreateMod("B", false) };
            var collections = new Mock<IModCollectionService>();
            collections.Setup(p => p.Get("w40k")).Returns(collection);
            var mods = new Mock<IModService>();
            mods.Setup(p => p.ResolveCollectionMods(It.IsAny<IEnumerable<IMod>>(), collection, null)).Returns(new[] { installed[1], installed[0] });

            var result = new CollectionModSource(collections.Object, mods.Object).Resolve("w40k", installed);

            result.Mods.Select(p => p.Name).Should().Equal("B", "A");
            result.Unmatched.Should().BeEmpty();
        }

        /// <summary>
        /// Defines the test method Resolve_reports_missing_mods_as_unmatched.
        /// </summary>
        [Fact]
        public void Resolve_reports_missing_mods_as_unmatched()
        {
            var collection = new Mock<IModCollection>().Object;
            var collections = new Mock<IModCollectionService>();
            collections.Setup(p => p.Get("w40k")).Returns(collection);
            var mods = new Mock<IModService>();
            mods.Setup(p => p.ResolveCollectionMods(It.IsAny<IEnumerable<IMod>>(), collection, null)).Returns(new[] { CreateMod("A", false), CreateMod("Gone", true, 1591095271) });

            var result = new CollectionModSource(collections.Object, mods.Object).Resolve("w40k", []);

            result.Mods.Select(p => p.Name).Should().Equal("A");
            result.Unmatched.Should().Equal(new PlaylistEntry("Gone", true, 1, "1591095271", null));
        }

        /// <summary>
        /// Defines the test method Resolve_unknown_collection_throws.
        /// </summary>
        [Fact]
        public void Resolve_unknown_collection_throws()
        {
            var collections = new Mock<IModCollectionService>();
            collections.Setup(p => p.Get(It.IsAny<string>())).Returns((IModCollection)null);

            var act = () => new CollectionModSource(collections.Object, new Mock<IModService>().Object).Resolve("nope", []);

            act.Should().Throw<CliInputException>().WithMessage("*nope*");
        }

        /// <summary>
        /// Creates the mod.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isVirtual">if set to <c>true</c> the mod is not installed.</param>
        /// <param name="remoteId">The remote identifier.</param>
        /// <returns>IMod.</returns>
        private static IMod CreateMod(string name, bool isVirtual, long? remoteId = null)
        {
            var mod = new Mock<IMod>();
            mod.SetupProperty(p => p.Name, name);
            mod.SetupProperty(p => p.IsVirtual, isVirtual);
            mod.SetupProperty(p => p.RemoteId, remoteId);
            return mod.Object;
        }

        #endregion Methods
    }
}
