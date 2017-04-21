using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    /// <summary>
    /// Allows specifying a namespace for importing into a code file.
    /// </summary>
    public class UsingDeclaration : IComparable<UsingDeclaration>, IEquatable<UsingDeclaration>
    {
        #region Static

        public static UsingDeclaration System
        {
            get
            {
                return new UsingDeclaration(Namespaces.SystemNamespaceName);
            }
        }
        public static UsingDeclaration SystemCollectionsGeneric
        {
            get
            {
                return new UsingDeclaration(Namespaces.SystemCollectionsGenericNamespaceName);
            }
        }
        public static UsingDeclaration SystemLinq
        {
            get
            {
                return new UsingDeclaration(Namespaces.SystemLinq);
            }
        }
        public static UsingDeclaration SystemText
        {
            get
            {
                return new UsingDeclaration(Namespaces.SystemText);
            }
        }


        public static bool operator ==(UsingDeclaration lhs, UsingDeclaration rhs)
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

        public static bool operator !=(UsingDeclaration lhs, UsingDeclaration rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region IEquatable<UsingDeclaration> Members

        public bool Equals(UsingDeclaration other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

#if DEBUG
            bool output = true;
            output = output && (this.ToString() == other.ToString());

            return output;
#else
            return
                (this.ToString() == other.ToString())
#endif
        }

        #endregion


        public string NamespaceName { get; set; }


        public UsingDeclaration()
        {
        }

        public UsingDeclaration(string namespaceName)
        {
            this.NamespaceName = namespaceName;
        }

        public int CompareTo(UsingDeclaration other)
        {
            return this.NamespaceName.CompareTo(other.NamespaceName);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as UsingDeclaration);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return this.NamespaceName;
        }
    }
}
