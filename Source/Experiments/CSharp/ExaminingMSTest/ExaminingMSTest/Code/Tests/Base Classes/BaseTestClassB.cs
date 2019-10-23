using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExaminingMSTest
{
    /// <summary>
    /// An abstract base test class WITH the <see cref="TestClassAttribute"/>.
    /// </summary>
    [TestClass]
    public abstract class BaseTestClassB
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Console.WriteLine($"{nameof(BaseTestClassB)}.{nameof(ClassInitialize)}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine($"{nameof(BaseTestClassB)}.{nameof(ClassCleanup)}");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine($"{nameof(BaseTestClassB)}.{nameof(TestInitialize)}");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine($"{nameof(BaseTestClassB)}.{nameof(TestCleanup)}");
        }

        [TestMethod]
        public void TestB()
        {
            Console.WriteLine($"{nameof(BaseTestClassB)}.{nameof(TestB)}");
        }
    }
}
