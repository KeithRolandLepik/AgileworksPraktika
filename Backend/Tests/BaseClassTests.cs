using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class BaseClassTests<TClass, TBaseClass> : BaseTests
    {
            protected TClass Object;
            [TestInitialize]
            public virtual void TestInitialize()
            {
                Type = typeof(TClass);
                Fixture = new Fixture();

        }

            [TestMethod]
            public void IsInheritedTest()
            {
                Assert.AreEqual(typeof(TBaseClass), Type.BaseType);
            }

        }
    }