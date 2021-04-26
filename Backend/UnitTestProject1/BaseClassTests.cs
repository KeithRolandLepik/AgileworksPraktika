using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class BaseClassTests<TClass, TBaseClass> : BaseTests
    {
            protected TClass obj;
            [TestInitialize]
            public virtual void TestInitialize()
            {
                type = typeof(TClass);

            }
            [TestMethod]
            public void IsInheritedTest()
            {
                Assert.AreEqual(typeof(TBaseClass), type.BaseType);
            }

        }
    }