using System;

using Public.Common.Lib;


namespace Public.Examples.Code
{
    class EqualsClass : IEquatable<EqualsClass>
    {
        #region Static

        public static bool operator ==(EqualsClass lhs, EqualsClass rhs)
        {
            if(object.ReferenceEquals(null, lhs))
            {
                if(object.ReferenceEquals(null, rhs))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs); // Equals() handles a null right side.
        }

        public static bool operator !=(EqualsClass lhs, EqualsClass rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region IEquatable<EqualsClass> Members

        public bool Equals(EqualsClass other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if(object.ReferenceEquals(this, other))
            {
                return true;
            }

            if(this.GetType() != other.GetType())
            {
                return false;
            }

#if DEBUG
            bool output = true;
            output = output && (this.A == other.A);
            output = output && (this.B == other.B);

            return output;
#else
            return
                (this.A == other.A) &&
                (this.B == other.B);
#endif
        }

        #endregion


        public string A { get; set; }
        public string B { get; set; }


        public EqualsClass() { }

        public EqualsClass(string a, string b)
        {
            this.A = a;
            this.B = b;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EqualsClass);
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.A, this.B);
        }
    }


    class EqualsClassDescendant : EqualsClass, IEquatable<EqualsClassDescendant>
    {
        #region Static

        public static bool operator ==(EqualsClassDescendant lhs, EqualsClassDescendant rhs)
        {
            if (object.ReferenceEquals(null, lhs))
            {
                if (object.ReferenceEquals(null, rhs))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs); // Equals() handles a null right side.
        }

        public static bool operator !=(EqualsClassDescendant lhs, EqualsClassDescendant rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region IEquatable<EqualsClassDescendant> Members

        public bool Equals(EqualsClassDescendant other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if(object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.C == other.C)
            {
                return base.Equals((EqualsClass)other);
            }
            
            return false;
        }

        #endregion


        public string C { get; set; }


        public EqualsClassDescendant() : base() { }

        public EqualsClassDescendant(string a, string b, string c)
            : base(a, b)
        {
            this.C = c;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EqualsClassDescendant);
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(base.GetHashCode(), this.C);
        }
    }
}
