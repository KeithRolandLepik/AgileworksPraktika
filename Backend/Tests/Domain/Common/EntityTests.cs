using Data.Feedbacks;
using Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Domain.Common
{
    [TestClass]
    public class EntityTests : BaseClassTests<Entity<FeedbackData>, object>
    {
        private class testClass : Entity<FeedbackData>
        {
            public testClass(FeedbackData entityData = null) : base(entityData) { }
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new testClass();
        }

        [TestMethod]
        public void Entity_should_be_abstract()
        {
            // Assert
            Assert.IsTrue(Type.IsAbstract);
        }

        [TestMethod]
        public void New_entity_data_should_set_value_from_parameter()
        {
            var entityData = new FeedbackData { Id = 1, Description = "asd", IsCompleted = false, IsOverdue = false, DateAdded = DateTime.Now, DueDate = new DateTime(2021, 6, 6, 20, 40, 0) };

            // Act
            var initialEntityData = Object.Data;
            Object = new testClass(entityData);

            // Assert
            Assert.AreNotSame(entityData, initialEntityData);
            AssertArePropertyValuesEqual(Object.Data, entityData);
        }

        [TestMethod]
        public void Entity_data_should_be_null_before_setting_and_have_value_after_setting()
        {
            var entityData = new FeedbackData { Id = 1, Description = "asd", IsCompleted = false, IsOverdue = false, DateAdded = DateTime.Now, DueDate = new DateTime(2021, 6, 6, 20, 40, 0) };

            // Act
            var initialEntityData = Object.Data;
            Object.Data = entityData;

            // Assert
            Assert.IsNull(initialEntityData);
            AssertArePropertyValuesEqual(Object.Data, entityData);
        }

        [TestMethod]
        public void Entity_data_can_be_null()
        {
            // Act
            Object.Data = null;

            // Assert
            Assert.IsNull(Object.Data);
        }
    }
}
