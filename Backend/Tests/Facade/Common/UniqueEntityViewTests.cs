using Facade.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Facade.Common
{
    [TestClass]
    public class UniqueEntityViewTests : BaseClassTests<UniqueEntityView, object>
    {
        private class TestClass : UniqueEntityView { }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            obj = new TestClass();

        }
        [TestMethod]
        public void DescriptionTest()
        {
            Random rnd = new Random();
            var val = rnd.Next(1, 10);

            Assert.AreNotEqual(obj.Id, val);

            obj.Id = val;
            Assert.AreEqual(obj.Id, val);

        }
    }
}
