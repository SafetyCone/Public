using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExaminingMSTest
{
    /// <summary>
    /// Shows that the [ClassInitialize] and [ClassCleanup] static methods can call the base static methods.
    /// </summary>
    [TestClass]
    public class TestClassD : BaseTestClassB
    {
        [ClassInitialize]
        public static void ClassInitializeD(TestContext testContext)
        {
            BaseTestClassB.ClassInitialize(testContext);

            Console.WriteLine($"{nameof(TestClassD)}.{nameof(ClassInitializeD)}");
        }

        [ClassCleanup]
        public static void ClassCleanupD()
        {
            Console.WriteLine($"{nameof(TestClassD)}.{nameof(ClassCleanupD)}");

            BaseTestClassB.ClassCleanup();
        }

        [TestInitialize]
        public void TestInitializeD()
        {
            Console.WriteLine($"{nameof(TestClassD)}.{nameof(TestInitializeD)}");
        }

        [TestCleanup]
        public void TestCleanupD()
        {
            Console.WriteLine($"{nameof(TestClassD)}.{nameof(TestCleanupD)}");
        }
    }
}
