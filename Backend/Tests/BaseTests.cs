using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    public abstract class BaseTests
    {
        protected Type Type;
        protected static void TestArePropertyValuesEqual(object obj1, object obj2)
        {
            foreach (var property in obj1.GetType().GetProperties())
            {
                var propName = property.Name;
                var p = obj2.GetType().GetProperty(propName);
                Assert.IsNotNull(p, $"No property with name '{propName}' found.");
                var expected = property.GetValue(obj1);
                var actual = p.GetValue(obj2);
                Assert.AreEqual(expected, actual, $"For property '{propName}'.");
            }
        }
    }
}
