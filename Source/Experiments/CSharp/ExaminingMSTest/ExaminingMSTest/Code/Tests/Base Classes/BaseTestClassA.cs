using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExaminingMSTest
{
    /// <summary>
    /// An abstract base test class WITHOUT the <see cref="TestClassAttribute"/>.
    /// </summary>
    public abstract class BaseTestClassA
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Console.WriteLine($"{nameof(BaseTestClassA)}.{nameof(ClassInitialize)}");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine($"{nameof(BaseTestClassA)}.{nameof(ClassCleanup)}");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine($"{nameof(BaseTestClassA)}.{nameof(TestInitialize)}");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine($"{nameof(BaseTestClassA)}.{nameof(TestCleanup)}");
        }

        [TestMethod]
        public void TestA()
        {
            Console.WriteLine($"{nameof(BaseTestClassA)}.{nameof(TestA)}");
        }
    }
}
