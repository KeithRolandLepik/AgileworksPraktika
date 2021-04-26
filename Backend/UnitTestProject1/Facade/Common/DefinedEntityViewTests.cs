using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Facade.Common;

namespace Tests.Facade.Common
{
    [TestClass]
    public class DefinedEntityViewTests : BaseClassTests<DefinedEntityView, UniqueEntityView>
    {
        private class TestClass : DefinedEntityView { }
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
            var val = rnd.Next(1, 10).ToString();

            Assert.AreNotEqual(obj.Description, val);

            obj.Description = val;
            Assert.AreEqual(obj.Description, val);

        }
    }
}
