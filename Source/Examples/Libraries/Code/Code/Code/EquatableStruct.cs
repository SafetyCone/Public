using System;

using Public.Common.Lib;


namespace Public.Examples.Code
{
    // This struct has no example descendant because structs are sealed. Structs are value types, and value types cannot serve as the base type for another type.

    // IEquatable<T> should always be implemented for value types.
    // Make the struct serializable because why not?
    [Serializable]
    struct EquatableStruct : IEquatable<EquatableStruct>
    {
        #region Static

        // If object.Equals() is overriden, then the op_Equals() should be overriden, and vice-versa.
        // The compiler helpfully warns that these relationships should be followed.
        // If you want to have the compiler disable the warning in a particular file (and re-enable it later in the file).
        // Also, for ease of use, defining these operators can help.
        public static bool operator ==(EquatableStruct lhs, EquatableStruct rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(EquatableStruct lhs, EquatableStruct rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion

        #region IEquatable<ReferenceStruct> Members

        // Important NOT to interface-explicity implement this method since it will be needed in the Equals() override and operator overloads.
        // I.e., do NOT use bool IEquatable<ReferenceStruct>.Equals(ReferenceStruct other).
        public bool Equals(EquatableStruct other)
        {
            return (this.C == other.C) && (this.D == other.D);
        }

        #endregion


        //public string C { get; set; }
        //public string D { get; set; }

        // Structure should be made immutable.
        private readonly string zC;
        private readonly string zD;

        public string C
        {
            get
            {
                return zC;
            }
        }
        public string D
        {
            get
            {
                return zD;
            }
        }


        // A default constructor is not allowed.
        //public ReferenceStruct() { }

        //public EqualsStruct(string c, string d)
        //    : this() // Must include call to parameterless constructor before using the reference 'this'.
        //{
        //    this.C = c;
        //    this.D = d;
        //}

        // Or just avoid using the reference this.
        public EquatableStruct(string c, string d)
        {
            this.zC = c;
            this.zD = d;
        }

        // Structs do not need a copy constructor or assignment operator.
        // The protected Object.MemberwiseClone() method does a direct memory-copy to a new memory location.
        // Thus assignment IS a copy constructor.
        // If mutable (unreccommended) and used as a property or in a collection (like List), there is no way to change the values of the struct, since a copy is always returned by the property or collection.
        // The only way to change values is to set the property or collection item to be a new instance of the structure, containing new values set by a constructor.
        //public EqualsStruct(EqualsStruct other)
        //{
        //    // NOPE!
        //}

        // For value-types, the default equals should be overridden if the value-type has reference type fields.
        // If none of the fields in a value type are reference types, then the default equals method performs a byte-by-byte comparison on the objects. This is fast, and thus there is no reason to override Object.Equals().
        // However, if there are reference fields in the structure, the default equals method uses reflection to test equality of those fields. This is very very slow, and thus if there are reference fields in the value type object, you had better override Object.Equals().
        // But, what types are value types and what types are reference types? Is string a value or reference type?
        // You can go to the type's definition (either right-click->Go To Definition or F12), and if the type is defined as a class, it's a reference type, if it's defined as a struct, it's a value type.
        // Using this method, we can see that strings are reference types. But! They are implemented to behave like values types.
        // DateTime is actually a value type, even though it is a very complicated object so one might think it's a reference type.
        //
        // Generally, it's not hard to override Object.Equals() so just do it.
        public override bool Equals(object obj)
        {
            // Note that because structs are implicitly sealed (no descendent classes) instead of using 'as' we can use 'is'.
            if (obj is EquatableStruct objAsEqualsStruct)
            {
                return this.Equals(objAsEqualsStruct);
            }
            return false;
        }

        // If object.Equals() is overridden, then object.GetHashCode() should also be overriden.
        //
        // For a struct, getting a good hash code is complicated.
        // It depends on whether the struct contains any reference types, and then whether there is padding between fields.
        // Padding occurs when the compiler tries to align fields on particular byte boundaries. This padding process is complicated to predict, and is generally only empirically possible via the use of System.Runtime.InteropServices.Marshal.SizeOf(). NOTE: this can even change based on the CPU!
        // 
        // For value types without reference fields or padding (a 50/50 occurrence), a very fast 32-bit at a time XOR is performed.
        // However, if things are not perfect, reflection is used to find the first usable field (non-static, non-null) and only the hash of that field is used as the object's hash...!
        // This may not be good, since what if the field that really differentiates between instances of a structure is the second field! Then the hash-table is basically a linear lookup.
        //
        // If you don't care about the time penalty, OR the structure will never be used as a key in a lookup table, then who cares? Otherwise, override GetHashCode().
        // Generally, very little time is consumed in calculating hash codes, so until profiling shows it to be a bottle-neck, just do it.
        // Especially with the HashHelper class that makes it easy to implement a hash function.
        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.C, this.D);
            //int output = HashHelperPrimeMultiplier.GetHashCode(this.C, this.D);
            return output;
        }
    }
}
