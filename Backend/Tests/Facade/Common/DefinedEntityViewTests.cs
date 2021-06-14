using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Facade.Common;

namespace Tests.Facade.Common
{
    [TestClass]
    public class DefinedEntityViewTests : BaseClassTests<DefinedEntityModel, UniqueEntityModel>
    {
        private class TestClass : DefinedEntityModel { }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new TestClass();
        }

        [TestMethod]
        public void Description_should_be_gettable_and_settable()
        {
            var randomDescriptionValue = GetRandom.RndInteger(1,100).ToString();

            // Act
            var initialDescriptionValue = Object.Description;
            Object.Description = randomDescriptionValue;

            // Assert
            Assert.AreNotEqual(Object.Description, initialDescriptionValue);
            Assert.AreEqual(Object.Description, randomDescriptionValue);
        }
    }
}
