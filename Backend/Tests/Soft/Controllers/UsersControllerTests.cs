using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Data.Feedbacks;
using Facade.Users;
using Infra.Authentication;
using Infra.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        private void AddUsers()
        {
            for (int i = 0; i < Count; i++)
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
            Controller.Register(userRequest).GetAwaiter();
        }
        [TestMethod]
        public void Authenticate()
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
            // Assert
            Assert.AreEqual(typeof(BadRequestObjectResult), response1.GetType());
            Assert.AreEqual(typeof(OkObjectResult),response2.GetType());
        }

        [TestMethod]
        public void Delete()
        {
            var countBefore = Controller.GetAll().GetAwaiter().GetResult().Value.Count;
            // Act
            var result1 = Controller.Delete(Fixture.Create<int>() * Fixture.Create<int>() ).GetAwaiter().GetResult();
            var countAfter = Controller.GetAll().GetAwaiter().GetResult().Value.Count;

            var result2 = Controller.Delete(_userData.Id).GetAwaiter().GetResult();
            var finalCount = Controller.GetAll().GetAwaiter().GetResult().Value.Count;
            // Assert
            Assert.AreEqual(result1.GetType(), typeof(BadRequestResult));
            Assert.AreEqual(countBefore,countAfter);

            Assert.AreEqual(result2.GetType(),typeof(OkResult));
            Assert.AreEqual(countAfter - finalCount, 1);
        }

        [TestMethod]
        public void GetAll()
        {

            // Act
            // Assert
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetById()
        {

            // Act
            // Assert
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Update()
        {
            // Act
            // Assert
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Register()
        {
            // Act
            // Assert
            Assert.Inconclusive();
        }
    }
}
