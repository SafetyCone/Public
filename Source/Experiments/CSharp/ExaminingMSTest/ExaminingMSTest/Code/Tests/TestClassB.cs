using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExaminingMSTest
{
    /// <summary>
    /// An test class derived from a test class WITH the <see cref="TestClassAttribute"/>.
    /// Shows that re-use of special MSTest method names confuses MSTest and leads to MSTest not calling the base class names!
    /// </summary>
    /// <remarks>
    /// Use of 'new' on all methods is required to avoid C# compiler warnings. HOWEVER, MSTest does not recognize the 'new' methods!
    /// </remarks>
    [TestClass]
    public class TestClassB : BaseTestClassB
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(ClassInitialize)}");
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(ClassCleanup)}");
        }

        [TestInitialize]
        public new void TestInitialize()
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(TestInitialize)}");
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            Console.WriteLine($"{nameof(TestClassB)}.{nameof(TestCleanup)}");
        }
    }
}
