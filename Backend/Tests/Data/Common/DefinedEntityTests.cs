using Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            var randomValue = GetRandom.RndInteger(1, 10).ToString();

            // Act
            var initialDescriptionValue = Object.Description;
            Object.Description = randomValue;

            // Assert
            Assert.AreNotEqual(initialDescriptionValue, randomValue);
            Assert.AreEqual(Object.Description, randomValue);
        }
    }
}
