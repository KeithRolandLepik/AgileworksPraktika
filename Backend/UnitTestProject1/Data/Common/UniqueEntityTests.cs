using Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Data.Common
{
    [TestClass]
    public class UniqueEntityTests : BaseClassTests<UniqueEntityData, object>
    {
        private class TestClass : UniqueEntityData { }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            obj = new TestClass();

        }
        [TestMethod]
        public void IsAbstract()
        {
            Assert.IsTrue(type.IsAbstract);
        }
        [TestMethod]
        public void IdTest()
        {
            Random rnd = new Random();
            var val = rnd.Next(1, 10);

            Assert.AreNotEqual(obj.Id, val);

            obj.Id = val;
            Assert.AreEqual(obj.Id, val);

        }
    }
}
