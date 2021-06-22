using AutoFixture;
using Data.Common;
using Data.Feedbacks;
using Data.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Data.Users
{
    [TestClass]
    public class UserDataTests : BaseClassTests<UserData, UniqueEntityData>
    {
        private class TestClass : UserData { }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new TestClass();
            Fixture = new Fixture();
        }

        [TestMethod]
        public void FirstName_should_be_gettable_and_settable()
        {
            var randomValue = Fixture.Create<string>();

            // Act
            var initialValue = Object.FirstName;
            Object.FirstName = randomValue;

            // Assert
            Assert.AreNotEqual(initialValue, randomValue);
            Assert.AreEqual(Object.FirstName, randomValue);

        }

        [TestMethod]
        public void LastName_should_be_gettable_and_settable()
        {
            var randomValue = Fixture.Create<string>();

            // Act
            var initialValue = Object.LastName;
            Object.LastName = randomValue;

            // Assert
            Assert.AreNotEqual(initialValue, randomValue);
            Assert.AreEqual(Object.LastName, randomValue);
        }

        [TestMethod]
        public void UserName_should_be_gettable_and_settable()
        {
            var randomValue = Fixture.Create<string>();

            // Act
            var initialValue = Object.Username;
            Object.Username = randomValue;

            // Assert
            Assert.AreNotEqual(initialValue, randomValue);
            Assert.AreEqual(Object.Username, randomValue);
        }
    }
}
