using System;

using Public.Common.Lib;


namespace Public.Examples.Code
{
    // This struct has no example descendant because structs are sealed. Structs are value types, and value types cannot serve as the base type for another type.

    // IEquatable<T> should always be implemented for value types.
    struct EqualsStruct : IEquatable<EqualsStruct>
    {
        #region Static
        
        // If object.Equals() is overriden, then the op_Equals() should be overriden.
        public static bool operator ==(EqualsStruct lhs, EqualsStruct rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present.
        public static bool operator !=(EqualsStruct lhs, EqualsStruct rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion

        #region IEquatable<ReferenceStruct> Members

        //bool IEquatable<ReferenceStruct>.Equals(ReferenceStruct other) // Important NOT to explicity implement this method since it will be needed in the Equals() override and operator overloads.
        public bool Equals(EqualsStruct other)
        {
            return (this.C == other.C) && (this.D == other.D);
        }

        #endregion


        public string C { get; set; }
        public string D { get; set; }


        // Not allowed.
        //public ReferenceStruct() { }

        public EqualsStruct(string c, string d)
            : this() // Must include call to parameterless constructor for backing fields.
        {
            this.C = c;
            this.D = d;
        }

        // For value-types, it's recommended to override the default equals if the value-type has reference type fields.
        // However, are string value or reference types? They are reference types that behave like values types, but they are reference types indeed.
        // In general, always override Equals() for value types.
        public override bool Equals(object obj)
        {
            if (obj is EqualsStruct)
            {
                return this.Equals((EqualsStruct)obj);
            }
            return false;
        }

        // If object.Equals() is overridden, then object.GetHashCode() should also be overriden.
        // For value types, reflection over properties is again used to calculate hash codes.
        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.C, this.D);
        }
    }
}
