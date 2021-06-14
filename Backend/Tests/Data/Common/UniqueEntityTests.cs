using Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Data.Common
{
    [TestClass]
    public class UniqueEntityTests : BaseClassTests<UniqueEntityData, object>
    {
        private class TestClass : UniqueEntityData { }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new TestClass();
        }

        [TestMethod]
        public void UniqueEntityData_should_be_abstract()
        {
            // Assert
            Assert.IsTrue(Type.IsAbstract);
        }

        [TestMethod]
        public void Id_should_be_gettable_and_settable()
        {
            var randomValue = GetRandom.RndInteger(1, 10);

            // Act
            var initialIdValue = Object.Id;
            Object.Id = randomValue;

            // Assert
            Assert.AreNotEqual(initialIdValue, randomValue);
            Assert.AreEqual(Object.Id, randomValue);
        }
    }
}
