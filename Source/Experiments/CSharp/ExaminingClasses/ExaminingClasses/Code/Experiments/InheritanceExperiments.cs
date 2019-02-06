using System;

using ExaminingClasses.Lib;


namespace ExaminingClasses
{
    public class InheritanceExperiments
    {
        public static void SubMain()
        {
            //InheritanceExperiments.InterfaceImplementationInheritanceDispatch();
            //InheritanceExperiments.AsBaseInheritanceDispatch();
            InheritanceExperiments.CastToBaseInheritanceDispatch();
        }

        /// <summary>
        /// Result: UNEXPECTED! Casting just creates a reference to the object as a base-object. Virtual methods are still resolved based on the object's actual type!
        /// The 'as' operator just creates a reference to the derived-object as the base-object. Perhaps casting has a different behavior?
        /// Expected: Uses the virtual method implementation of the casted-type.
        /// </summary>
        private static void CastToBaseInheritanceDispatch()
        {
            ClassB B = new ClassB();

            var BCastToA = (ClassA)B;
            BCastToA.MethodA(); // ClassB:MethodA
        }

        /// <summary>
        /// Result: UNEXPECTED! Using the 'as' operator just creates a reference to the object as a base-object. Virtual methods are still resolved based on the object's actual type!
        /// If you have an object of a derived-type as a reference to a base-type, how is a virtual method dispatched?
        /// Expected: Uses the virtual method implementation of the reference type.
        /// </summary>
        private static void AsBaseInheritanceDispatch()
        {
            ClassB B = new ClassB();

            var BasA = B as ClassA; // var is ClassA.
            BasA.MethodA(); // ClassB:MethodA
        }

        /// <summary>
        /// Result: As expected, interface method dispatch based on actual type.
        /// If a class in an inheritance hierarchy implements an interface with a virtual method, how is the method dispatched?
        /// Expected: Dispatched based on the actual type of the object behind the interface.
        /// </summary>
        private static void InterfaceImplementationInheritanceDispatch()
        {
            IInterfaceA IA = new ClassB();

            IA.MethodA(); // ClassB:MethodA
        }
    }
}
