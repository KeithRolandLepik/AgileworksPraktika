using AutoFixture;
using Data.Feedbacks;
using Data.Users;
using Domain.Users;
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
            Assert.AreEqual(userRequest.Username, user.Data.Username);
            Assert.AreEqual(userRequest.LastName, user.Data.LastName);
            Assert.AreEqual(userRequest.FirstName, user.Data.FirstName);
        }

        [TestMethod]
        public void MapDomainToModel_should_map_domain_to_model()
        {
            var userData = Fixture.Create<UserData>();

            // Act
            var model = UserMapper.MapDomainToModel(new User(userData), string.Empty);

            // Assert
            Assert.AreEqual(userData.Username, model.Username);
            Assert.AreEqual(userData.LastName, model.LastName);
            Assert.AreEqual(userData.FirstName, model.FirstName);
        }
    }
}
