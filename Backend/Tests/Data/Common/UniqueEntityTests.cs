using Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoFixture;

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
            Fixture = new Fixture();
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
            var randomValue = Fixture.Create<int>();

            // Act
            var initialIdValue = Object.Id;
            Object.Id = randomValue;

            // Assert
            Assert.AreNotEqual(initialIdValue, randomValue);
            Assert.AreEqual(Object.Id, randomValue);
        }
    }
}
