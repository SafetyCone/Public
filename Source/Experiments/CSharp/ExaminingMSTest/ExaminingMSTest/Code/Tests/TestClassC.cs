using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExaminingMSTest
{
    /// <summary>
    /// Shows that for MSTest to follow inheritance rules with [TestIntialize] and [TestCleanup] methods, a test class MUST derive from a test class WITH the <see cref="TestClassAttribute"/> (marked abstract to prevent the base class from appearing in the Visual Studio Test Explorer window),
    /// AND has different special method names for base and derived classes (no use of the 'new' attribute) so that MSTest does not get confused.
    /// Thus to get inheritance of the [TestIntialize] and [TestCleanup] methods, the base class must be marked as a [TestClass], and special MSTest method names MUST be clas-specific.
    /// </summary>
    [TestClass]
    public class TestClassC : BaseTestClassB
    {
        [ClassInitialize]
        public static void ClassInitializeC(TestContext testContext)
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(ClassInitializeC)}");
        }

        [ClassCleanup]
        public static void ClassCleanupC()
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(ClassCleanupC)}");
        }

        [TestInitialize]
        public void TestInitializeC()
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(TestInitializeC)}");
        }

        [TestCleanup]
        public void TestCleanupC()
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(TestCleanupC)}");
        }
    }
}
