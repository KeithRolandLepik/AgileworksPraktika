using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Data.Users;
using Domain.Users;
using Facade.Users;
using Infra.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Soft.Controllers;

namespace Tests.Soft.Controllers
{
    [TestClass]
    public class MockUsersControllerTests : BaseTests
    {
        internal IOptions<AppSettings> AppSettings;
        internal UsersController Controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
            var app = new AppSettings { Secret = "asfdiowejkfoasdkfoekfa" };
            var mock = new Mock<IOptions<AppSettings>>();
            mock.Setup(x => x.Value).Returns(app);
            AppSettings = mock.Object;
        }

        [TestMethod]
        public void Authenticate_should_return_okObjectResult_with_valid_token_if_authenticated()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var userData = Fixture.Create<UserData>();
            var password = Fixture.Create<string>();
            var actualUserRequest = new UserRequest
            {
                FirstName = userData.FirstName,
                Id = userData.Id,
                LastName = userData.LastName,
                Password = password,
                Username = userData.Username
            };
            mockRepository.Setup(x =>
                x.Authenticate(actualUserRequest.Username, actualUserRequest.Password)).ReturnsAsync(AuthenticateResult.Success(new User(userData)));

            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.Authenticate(actualUserRequest).GetAwaiter().GetResult();
            var value = ((UserModel)((OkObjectResult)response2.Result).Value);

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), ((OkObjectResult)response2.Result).GetType());
            Assert.AreNotEqual(value.Token, string.Empty);
        }

        [TestMethod]
        public void Authenticate_should_return_badRequest_if_user_not_authenticated()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var userRequest = Fixture.Create<UserRequest>();

            mockRepository.Setup(x =>
                    x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthenticateResult.Failure("fail"));

            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.Authenticate(userRequest).GetAwaiter().GetResult().Result;

            // Assert
            Assert.AreEqual(typeof(BadRequestResult), ((BadRequestResult)response2).GetType());
        }

        [TestMethod]
        public void Register_should_return_userModel_object_with_a_token()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var userRequest = Fixture.Create<UserRequest>();
            var userData = Fixture.Create<UserData>();
            var user = new User(userData);

            mockRepository.Setup(x =>
                    x.Create(It.IsAny<User>(), userRequest.Password))
                .ReturnsAsync(user);

            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.Register(userRequest).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), response2.Result.GetType());
            Assert.AreNotEqual(((UserModel)((OkObjectResult)response2.Result).Value).Token, string.Empty);
        }

        [TestMethod]
        public void Getall_should_return_all_users_in_database()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var users = Fixture.CreateMany<UserData>(5).Select(x=> new User(x)).ToList();
            mockRepository.Setup(x => x.GetAll()).ReturnsAsync(users);
            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.GetAll().GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(5, response2.Value.Count);
        }

        [TestMethod]
        public void GetById_should_return_okObjectResult_and_usermodel_with_empty_token()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var user = new User(Fixture.Create<UserData>());

            mockRepository.Setup(x => x.GetById(user.Data.Id)).ReturnsAsync(user);
            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = (OkObjectResult)Controller.GetById(user.Data.Id).GetAwaiter().GetResult().Result;

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), response2.GetType());
            Assert.AreEqual(string.Empty, ((UserModel)response2.Value).Token);
        }

        [TestMethod]
        public void GetById_should_return_notFoundResult()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            mockRepository.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((User)null);
            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.GetById(Fixture.Create<int>()).GetAwaiter().GetResult().Result;

            // Assert
            Assert.AreEqual(typeof(NotFoundResult), response2.GetType());
        }

        [TestMethod]
        public void Update_should_return_okResult()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var userRequest = Fixture.Create<UserRequest>();
            var userData = Fixture.Create<UserData>();
            mockRepository.Setup(x => x.Update(It.IsAny<User>(),userRequest.Password)).Returns(Task.CompletedTask);
            Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.Update(userRequest.Id,userRequest).GetAwaiter().GetResult();

            // Assert
            mockRepository.Verify(x => x.Update(It.IsAny<User>(),userRequest.Password),Times.Once);
            Assert.AreEqual(typeof(OkResult), response2.GetType());
        }

        [TestMethod]
        public void Update_should_return_badRequest()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var userRequest = Fixture.Create<UserRequest>();
               Controller = new UsersController(mockRepository.Object, AppSettings);

            // Act
            var response2 = Controller.Update(Fixture.Create<int>(), userRequest).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(typeof(BadRequestResult), response2.GetType());
        }

        [TestMethod]
        public void Delete_should_return_okResult_if_entity_is_in_database()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            var id = Fixture.Create<int>();
            mockRepository.Setup(x =>
                x.Delete(id)).Returns(Task.CompletedTask);
            mockRepository.Setup(x =>
                x.GetById(id)).ReturnsAsync(new User(Fixture.Create<UserData>()));
            Controller = new UsersController(mockRepository.Object,AppSettings);

            // Act
            var result1 = Controller.Delete(id).GetAwaiter().GetResult();

            // Assert
            mockRepository.Verify(x => x.Delete(id), Times.Once);
            Assert.AreEqual(result1.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void Delete_should_return_notFoundResult_if_entity_is_not_in_database()
        {
            var mockRepository = new Mock<IUsersRepository> { DefaultValue = DefaultValue.Mock };
            mockRepository.Setup(x =>
                x.GetById(It.IsAny<int>())).ReturnsAsync((User)null);
            Controller = new UsersController(mockRepository.Object,AppSettings);

            // Act
            var result2 = Controller.Delete(Fixture.Create<int>()).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }
    }
}
