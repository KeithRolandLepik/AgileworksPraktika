using AutoFixture;
using Facade.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Facade.Users
{
    [TestClass]
    public class UserMapperTests : BaseTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
        }

        [TestMethod]
        public void MapToDomain_should_map_request_to_domain()
        {
            var userRequest = Fixture.Create<UserRequest>();

            // Act
            var user = UserMapper.MapRequestToDomain(userRequest); 

            // Assert
            Assert.AreEqual(userRequest.Username,user.Data.Username);
            Assert.AreEqual(userRequest.LastName, user.Data.LastName);
            Assert.AreEqual(userRequest.FirstName, user.Data.FirstName);
        }
    }
}
