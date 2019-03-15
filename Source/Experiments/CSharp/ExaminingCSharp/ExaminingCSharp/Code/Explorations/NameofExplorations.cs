using System;


namespace ExaminingCSharp
{
    public static class NameofExplorations
    {
        public static void SubMain()
        {
            //NameofExplorations.Classes();
            //NameofExplorations.Methods();
            NameofExplorations.Variables();
        }

        /// <summary>
        /// Explores the output of the nameof-operator on variables.
        /// 
        /// Conclusions:
        /// * Really, the output is just what you would expect, the name of the variable!
        /// </summary>
        private static void Variables()
        {
            var writer = Console.Out;

            var nameofWriter = nameof(writer);
            writer.WriteLine($@"nameof(writer): {nameofWriter}"); // writer.

            var obj = new object();
            var nameofObj = nameof(obj);
            writer.WriteLine($@"nameof(obj): {nameofObj}"); // obj.

            var str = @"A";
            var nameofStr = nameof(str);
            writer.WriteLine($@"nameof(str): {nameofStr}"); // str.
        }

        /// <summary>
        /// Explores the output of the nameof-operator on methods.
        /// 
        /// Conclusions:
        /// * Whether static or instance, the output is just the name of the method.
        /// </summary>
        private static void Methods()
        {
            var writer = Console.Out;

            var nameofStaticObjectEquals = nameof(Object.Equals);
            writer.WriteLine($@"nameof(Object.Equals) {nameofStaticObjectEquals}"); // Equals.

            var obj = new object();
            var nameofInstanceObjectEquals = nameof(obj.Equals);
            writer.WriteLine($@"nameof(obj.Equals) {nameofInstanceObjectEquals}"); // Equals.
        }

        /// <summary>
        /// Explores the output of the nameof-operator on classes.
        /// 
        /// Conclusions:
        /// * The namespace is not prefixed to the class name. The nameof output is just the name of the class. System.String is just String.
        /// * For generic types, the type parameter is not given. List{int} is just List.
        /// </summary>
        private static void Classes()
        {
            var writer = Console.Out;

            var nameofInt32 = nameof(Int32);
            writer.WriteLine($@"nameof(Int32): {nameofInt32}"); // Int32.

            var nameofSystemInt32 = nameof(System.Int32);
            writer.WriteLine($@"nameof(System.Int32: {nameofSystemInt32}"); // Int32.

            var nameofString = nameof(String);
            writer.WriteLine($@"nameof(String): {nameofString}"); // String.

            var nameofSystemString = nameof(System.String);
            writer.WriteLine($@"nameof(System.String): {nameofSystemString}"); // String.

            //var nameofList = nameof(System.Collections.Generic.List<>); // Cannot be done, unlike typeof(List<>).
            var nameofListInt32 = nameof(System.Collections.Generic.List<int>);
            writer.WriteLine($@"nameof(System.Collections.Generic.List<int>): {nameofListInt32}"); // List.
        }
    }
}
