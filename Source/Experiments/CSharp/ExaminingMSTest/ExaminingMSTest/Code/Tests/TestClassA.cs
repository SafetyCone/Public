using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExaminingMSTest
{
    /// <summary>
    /// An test class derived from a test class WITHOUT the <see cref="TestClassAttribute"/>.
    /// </summary>
    /// <remarks>
    /// Use of 'new' on all methods is required. Does this matter?
    /// </remarks>
    [TestClass]
    public class TestClassA : BaseTestClassA
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            Console.WriteLine($"{nameof(TestClassA)}.{nameof(ClassInitialize)}");
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            Console.WriteLine($"{nameof(TestClassA)}.{nameof(ClassCleanup)}");
        }

        [TestInitialize]
        public new void TestInitialize()
        {
            Console.WriteLine($"{nameof(TestClassA)}.{nameof(TestInitialize)}");
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            Console.WriteLine($"{nameof(TestClassA)}.{nameof(TestCleanup)}");
        }
    }
}
