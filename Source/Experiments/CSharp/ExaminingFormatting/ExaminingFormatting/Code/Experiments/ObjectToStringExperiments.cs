using System;

using ExaminingClasses.Lib;


namespace ExaminingFormatting
{
    public static class ObjectToStringExperiments
    {
        public static void SubMain()
        {
            //ObjectToStringExperiments.DefaultIsFullyQualifiedTypeName();
            ObjectToStringExperiments.CanOverrideDefault();
        }

        /// <summary>
        /// Result: ToString() overriding works.
        /// There is nothing magic about the ToString() method, it follows the same method behavior as all other methods, including allowing overriding virtual methods.
        /// Expected: Just the regular method behavior from the overrident ToString() method.
        /// </summary>
        private static void CanOverrideDefault()
        {
            var instance = OverrideToStringDemonstrator.NewExampleInstance();

            var toStringResult = instance.ToString(); // IntValue: 5, StringValue: '5'
            Console.WriteLine($@"ToString() result: {toStringResult}");
        }

        /// <summary>
        /// Result: The default Object.ToString() method outputs the fully-qualified name of the type.
        /// What exactly does the default Object.ToString() method that comes standard on every .NET class actually do?
        /// Expected: The fully-qualified type name.
        /// 
        /// As asserted here, the default Object.ToString() method outputs the name of the type: https://docs.microsoft.com/en-us/dotnet/standard/base-types/formatting-types#default-formatting-using-the-tostring-method
        /// </summary>
        private static void DefaultIsFullyQualifiedTypeName()
        {
            var instance = BasicClass.NewExampleInstance();

            var defaultToStringResult = instance.ToString(); // ExaminingFormatting.BasicClass
            Console.WriteLine($@"Default Object.ToString() result: {defaultToStringResult}.");

            var fullyQualifiedTypeName = instance.GetType().FullName; // ExaminingFormatting.BasicClass
            Console.WriteLine($@"Fully-qualified type-name: {fullyQualifiedTypeName}");

            var isEqual = defaultToStringResult == fullyQualifiedTypeName; // True.
            Console.Write($@"Are equal: {isEqual}");
        }
    }
}
