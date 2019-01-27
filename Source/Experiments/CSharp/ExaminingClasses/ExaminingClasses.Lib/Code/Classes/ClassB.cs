using System;


namespace ExaminingClasses.Lib
{
    /// <summary>
    /// Second-level class in an inheritance hierarchy.
    /// </summary>
    public class ClassB : ClassA, IInterfaceB
    {
        public double ValueTypeValueB { get; set; }
        public string ReferenceTypeValueB { get; set; }
    }
}
