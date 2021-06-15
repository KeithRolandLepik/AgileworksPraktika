﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Data.Feedbacks;
using Facade.Feedbacks;
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
            AddUser(_userData, _mockPassword);
        }

        private void AddUser(UserData user, string password)
        {
            var userRequest = new UserRequest
            {
                FirstName = _userData.FirstName, 
                Id = _userData.Id,
                LastName = _userData.LastName,
                Password = _mockPassword,
                Username = _userData.Username
            };
            _userData = Repository.Create(UserMapper.MapRequestToDomain(userRequest), userRequest.Password).Data;
        }

        [TestMethod]
        public void Authenticate_should_respond_null_if_incorrect_login_and_notnull_if_correct_login()
        {
            var password = Fixture.Create<string>();

            // Act
            var response = Repository.Authenticate(_userData.Username, password);
            var response2 = Repository.Authenticate(Fixture.Create<string>(),_userData.Username);
            var response3 = Repository.Authenticate(_userData.Username, _mockPassword);

            // Assert
            Assert.IsNull(response);
            Assert.IsNull(response2);
            Assert.IsNotNull(response3);
        }

        [TestMethod]
        public void GetAll_should_get_a_list_of_all_elements_in_database()
        {
            // Act
            var response = Repository.GetAll().ToList();

            // Assert
            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        public void GetById_should_return_user_by_id()
        {
            // Act
            var userData = Repository.GetById(_userData.Id);
            
            // Assert
            Assert.AreEqual(userData.Data.Username,_userData.Username);
            Assert.AreEqual(userData.Data.FirstName, _userData.FirstName);
            Assert.AreEqual(userData.Data.LastName, _userData.LastName);
        }

        [TestMethod]
        public void Create_should_add_an_object_to_database()
        {
            var countBefore = Repository.GetAll().Count();
            
            // Act
            Repository.Create(UserMapper.MapRequestToDomain(Fixture.Create<UserRequest>()), _mockPassword);

            // Assert
            var countAfter = Repository.GetAll().Count();
            Assert.AreEqual(countBefore,countAfter-1);
        }

        [TestMethod]
        public void Update_should_update_a_user_with_correct_id_and_password()
        {
            var newFeedbackUpdate = Fixture.Create<UserRequest>();

            // Act
            var initialUserData = Repository.GetById(_userData.Id);
            var userToUpdate = UserMapper.MapRequestToDomain(newFeedbackUpdate);
            userToUpdate.Data.Id = _userData.Id;
            Repository.Update(userToUpdate,_mockPassword);

            // Assert
            var initialUserDataCopy = Repository.GetById(_userData.Id);
            Assert.AreEqual(initialUserDataCopy.Data.Username, userToUpdate.Data.Username);
            Assert.AreEqual(initialUserDataCopy.Data.FirstName, userToUpdate.Data.FirstName);
            Assert.AreEqual(initialUserDataCopy.Data.LastName, userToUpdate.Data.LastName);

        }

        [TestMethod]
        public void Delete_should_remove_a_user_from_database()
        {
            var countBefore = Repository.GetAll().Count();

            // Act
            Repository.Delete(_userData.Id);
            var countAfter = Repository.GetAll().Count();

            // Assert
            Assert.AreEqual(countBefore-countAfter, 1);
        }
    }
}
