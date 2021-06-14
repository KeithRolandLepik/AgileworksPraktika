using Facade.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;

namespace Tests.Facade.Common
{
    [TestClass]
    public class UniqueEntityViewTests : BaseClassTests<UniqueEntityModel, object>
    {
        private class TestClass : UniqueEntityModel { }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new TestClass();
            Fixture = new Fixture();
        }

        [TestMethod]
        public void Id_should_be_gettable_and_settable()
        {
            var randomIdValue = Fixture.Create<int>();

            // Act
            var initialIdValue = Object.Id;
            Object.Id = randomIdValue;

            // Assert
            Assert.AreNotEqual(Object.Id, initialIdValue);
            Assert.AreEqual(Object.Id, randomIdValue);
        }
    }
}
