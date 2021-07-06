using AutoFixture;
using Data.Users;
using Facade.Users;
using Infra.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Soft.Controllers;
using Tests.Infra.Common;

namespace Tests.Soft.Controllers
{
    [TestClass]
    public class UsersControllerTests : DatabaseTestsBase
    {
        protected UsersRepository Repository;
        protected UsersController Controller;
        protected int Count;
        private UserData _userData;
        private string _mockPassword;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTestDatabase();
            Repository = new UsersRepository(DocumentStore);
            Controller = new UsersController(Repository, AppSettings);
            Fixture = new Fixture(); 
            _userData = Fixture.Create<UserData>();
            _mockPassword = Fixture.Create<string>();
            Count = 10;
            AddUsers();
            AddUser(_userData, _mockPassword);
        }

        [TestMethod]
        public void Authenticate_should_return_badRequestResult_on_not_registered_login_attempt_and_OkObjectResult_and_auth_token_on_registered_user()
        {
            var randomUserRequest = Fixture.Create<UserRequest>();
            var actualUserRequest = new UserRequest
            {
                FirstName = _userData.FirstName,
                Id = _userData.Id,
                LastName = _userData.LastName,
                Password = _mockPassword,
                Username = _userData.Username
            };

            // Act
            var response1 = Controller.Authenticate(randomUserRequest).GetAwaiter().GetResult();
            var response2 = Controller.Authenticate(actualUserRequest).GetAwaiter().GetResult();
            var value = ((UserModel)((OkObjectResult)response2.Result).Value);

            // Assert
            Assert.AreEqual(typeof(BadRequestResult), ((BadRequestResult)response1.Result).GetType());
            Assert.AreEqual(typeof(OkObjectResult), ((OkObjectResult)response2.Result).GetType());
            Assert.AreEqual(value.Id, _userData.Id);
            Assert.AreEqual(value.FirstName, _userData.FirstName);
            Assert.AreEqual(value.LastName, _userData.LastName);
            Assert.AreEqual(value.Username, _userData.Username);
            Assert.AreNotEqual(value.Token,string.Empty);
        }


        [TestMethod]
        public void Delete_should_remove_an_object_from_database()
        {
            var countBefore = Controller.GetAll().GetAwaiter().GetResult().Value.Count;

            // Act
            var result1 = Controller.Delete(Fixture.Create<int>() * Fixture.Create<int>() ).GetAwaiter().GetResult();
            var countAfter = Controller.GetAll().GetAwaiter().GetResult().Value.Count;

            var result2 = Controller.Delete(_userData.Id).GetAwaiter().GetResult();
            var finalCount = Controller.GetAll().GetAwaiter().GetResult().Value.Count;

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(NotFoundResult));
            Assert.AreEqual(countBefore,countAfter);

            Assert.AreEqual(result2.GetType(),typeof(OkResult));
            Assert.AreEqual(countAfter - finalCount, 1);
        }

        [TestMethod]
        public void GetAll_should_return_all_objects_in_database()
        {
            // Act
            var count = Controller.GetAll().GetAwaiter().GetResult().Value.Count;
            
            // Assert
            Assert.AreEqual(Count + 1, count);
        }

        [TestMethod]
        public void GetById_should_return_an_object_when_given_correct_id()
        {
            var randomId = Fixture.Create<int>();
            
            // Act
            var response1 = Controller.GetById(randomId).GetAwaiter().GetResult();
            var response2 = Controller.GetById(_userData.Id).GetAwaiter().GetResult();
            var value = ((UserModel) ((OkObjectResult) response2.Result).Value);

            // Assert
            Assert.AreEqual(((NotFoundResult)response1.Result).GetType(), typeof(NotFoundResult));
            Assert.AreEqual(((OkObjectResult)response2.Result).GetType(),typeof(OkObjectResult));

            Assert.AreEqual(value.Id, _userData.Id);
            Assert.AreEqual(value.FirstName, _userData.FirstName);
            Assert.AreEqual(value.LastName, _userData.LastName);
            Assert.AreEqual(value.Username, _userData.Username);
        }

        [TestMethod]
        public void Update_should_update_a_database_object_given_correct_username_and_password_and_id()
        {
            var userRequest = new UserRequest()
            {
                Id = _userData.Id,
                FirstName = Fixture.Create<string>(),
                LastName= Fixture.Create<string>(),
                Username= Fixture.Create<string>(),
                Password= _mockPassword,
            };

            // Act
            var result = Controller.Update(_userData.Id, userRequest).GetAwaiter().GetResult();
            var databaseUser =
                ((UserModel) ((OkObjectResult) Controller.GetById(_userData.Id).GetAwaiter().GetResult().Result).Value);

            // Assert
            Assert.AreEqual(result.GetType(),typeof(OkResult));
            Assert.AreEqual(userRequest.Id,databaseUser.Id);
            Assert.AreEqual(userRequest.FirstName, databaseUser.FirstName);
            Assert.AreEqual(userRequest.LastName, databaseUser.LastName);
            Assert.AreEqual(userRequest.Username, databaseUser.Username);
        }

        [TestMethod]
        public void Register_should_add_a_new_object_to_the_database_when_username_is_unique()
        {
            var userRequest = new UserRequest
            {
                FirstName = Fixture.Create<string>(),
                Id = Fixture.Create<int>(),
                LastName = Fixture.Create<string>(),
                Password = Fixture.Create<string>(),
                Username = Fixture.Create<string>()
            };

            // Act
            var result = (UserModel)((OkObjectResult)Controller.Register(userRequest).GetAwaiter().GetResult().Result).Value;
            var databaseUser = ((UserModel)((OkObjectResult)Controller.GetById(result.Id).GetAwaiter().GetResult().Result).Value);

            // Assert
            Assert.AreEqual(databaseUser.Id,result.Id);
            Assert.AreEqual(databaseUser.FirstName, result.FirstName);
            Assert.AreEqual(databaseUser.LastName, result.LastName);
            Assert.AreEqual(databaseUser.Username, result.Username);
        }
        private void AddUsers()
        {
            for (var i = 0; i < Count; i++)
            {
                AddUser(Fixture.Create<UserData>(), Fixture.Create<string>());
            }
        }
        private void AddUser(UserData user, string password)
        {
            var userRequest = new UserRequest
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Password = password,
                Username = user.Username
            };
            _userData.Id = ((UserModel)((OkObjectResult)Controller.
                Register(userRequest).GetAwaiter().GetResult().Result).Value).Id;
        }
    }
}
