using Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Data.Common
{
    [TestClass]
    public class DefinedEntityTests : BaseClassTests<DefinedEntityData, UniqueEntityData>
    {
        private class TestClass : DefinedEntityData { }
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
        public void DescriptionTest()
        {
            Random rnd = new Random();
            var val = rnd.Next(1, 10).ToString();

            Assert.AreNotEqual(obj.Description, val);

            obj.Description = val;
            Assert.AreEqual(obj.Description, val);

        }
    }
}
