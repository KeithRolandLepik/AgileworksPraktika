using System.Linq;
using AutoFixture;
using Data.Users;
using Domain.Users;
using Facade.Users;
using Infra.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Infra.Common;

namespace Tests.Infra.Authentication
{
    [TestClass]
    public class UsersRepositoryTests : RepositoryTests
    {
        protected UsersRepository Repository;
        private UserData _userData;
        private string _mockPassword;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTestDatabase();
            Repository = new UsersRepository(DocumentStore);
            Fixture = new Fixture();
            _userData = Fixture.Create<UserData>();
            _mockPassword = Fixture.Create<string>();
            AddUser(_userData);
        }

        private void AddUser(UserData user)
        {
            var userRequest = new UserRequest
            {
                FirstName = _userData.FirstName, 
                Id = _userData.Id,
                LastName = _userData.LastName,
                Password = _mockPassword,
                Username = _userData.Username
            };
            _userData = Repository.Create(UserMapper.MapRequestToDomain(userRequest), userRequest.Password).GetAwaiter().GetResult()?.Data;
        }

        [TestMethod]
        public void Authenticate_should_return_authenticateResult_error_wrong_password_if_incorrect_login()
        {
            var password = Fixture.Create<string>();

            // Act
            var response = Repository.Authenticate(_userData.Username, password).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(response.ErrorMessage,"Wrong password");
        }

        [TestMethod]
        public void Authenticate_should_return_authenticateResult_error_user_not_found_if_incorrect_login()
        {
            // Act
            var response2 = Repository.Authenticate(Fixture.Create<string>(), _userData.Username).GetAwaiter().GetResult();
            
            // Assert
            Assert.AreEqual(response2.ErrorMessage, "User not found");
        }

        [TestMethod]
        public void Authenticate_should_return_authenticateResult_user_if_correct_login()
        {
            // Act
             var response3 = Repository.Authenticate(_userData.Username, _mockPassword).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(response3.User);
        }

        [TestMethod]
        public void GetAll_should_get_a_list_of_all_elements_in_database()
        {
            // Act
            var response = Repository.GetAll().GetAwaiter().GetResult().ToList();

            // Assert
            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        public void GetById_should_return_user_by_id()
        {
            // Act
            var userData = Repository.GetById(_userData.Id).GetAwaiter().GetResult();
            
            // Assert
            Assert.AreEqual(userData.Data.Username,_userData.Username);
            Assert.AreEqual(userData.Data.FirstName, _userData.FirstName);
            Assert.AreEqual(userData.Data.LastName, _userData.LastName);
        }

        [TestMethod]
        public void GetById_should_return_null_if_user_not_found()
        {
            // Act
            var userData = Repository.GetById(Fixture.Create<int>()).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(userData);
        }

        [TestMethod]
        public void Create_should_add_an_object_to_database()
        {
            var countBefore = Repository.GetAll().GetAwaiter().GetResult().Count();
            
            // Act
            Repository.Create(UserMapper.MapRequestToDomain(Fixture.Create<UserRequest>()), _mockPassword).GetAwaiter().GetResult();

            // Assert
            var countAfter = Repository.GetAll().GetAwaiter().GetResult().Count();
            Assert.AreEqual(countBefore,countAfter-1);
        }

        [TestMethod]
        public void Create_should_return_null_if_password_empty()
        {
            
            // Act
            var result = Repository.Create(UserMapper.MapRequestToDomain(Fixture.Create<UserRequest>()), string.Empty).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Create_should_return_null_if_user_already_in_database()
        {

            // Act
            var result = Repository.Create(new User(_userData), _mockPassword).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Update_should_update_a_user_with_correct_id_and_password()
        {
            var newFeedbackUpdate = Fixture.Create<UserRequest>();

            // Act
            var userToUpdate = UserMapper.MapRequestToDomain(newFeedbackUpdate);
            userToUpdate.Data.Id = _userData.Id;
            Repository.Update(userToUpdate,_mockPassword).GetAwaiter().GetResult();

            // Assert
            var initialUserDataCopy = Repository.GetById(_userData.Id).GetAwaiter().GetResult();
            Assert.AreEqual(initialUserDataCopy.Data.Username, userToUpdate.Data.Username);
            Assert.AreEqual(initialUserDataCopy.Data.FirstName, userToUpdate.Data.FirstName);
            Assert.AreEqual(initialUserDataCopy.Data.LastName, userToUpdate.Data.LastName);

        }

        [TestMethod]
        public void Delete_should_remove_a_user_from_database()
        {
            var countBefore = Repository.GetAll().GetAwaiter().GetResult().Count;

            // Act
            Repository.Delete(_userData.Id).GetAwaiter().GetResult();
            var countAfter = Repository.GetAll().GetAwaiter().GetResult().Count;

            // Assert
            Assert.AreEqual(countBefore-countAfter, 1);
        }
    }
}
