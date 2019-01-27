using System;


namespace ExaminingClasses.Lib
{
    public interface IInterfaceA
    {
        int ValueTypeValueA { get; set; } // Value-type (Int32) property.
        string ReferenceTypeValueA { get; set; } // Reference-type (String, even though it acts like a value-type) property.
    }
}
