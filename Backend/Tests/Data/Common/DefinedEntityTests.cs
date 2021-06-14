using Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoFixture;

namespace Tests.Data.Common
{
    [TestClass]
    public class DefinedEntityTests : BaseClassTests<DefinedEntityData, UniqueEntityData>
    {
        private class TestClass : DefinedEntityData { }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new TestClass();
            Fixture = new Fixture();
        }

        [TestMethod]
        public void DefinedEntityData_should_be_abstract()
        {
            // Assert
            Assert.IsTrue(Type.IsAbstract);
        }

        [TestMethod]
        public void Description_should_be_settable_and_gettable()
        {
            var randomValue = Fixture.Create<string>();

            // Act
            var initialDescriptionValue = Object.Description;
            Object.Description = randomValue;

            // Assert
            Assert.AreNotEqual(initialDescriptionValue, randomValue);
            Assert.AreEqual(Object.Description, randomValue);
        }
    }
}
