using System;


namespace ExaminingEquality.Examples
{
    /// <summary>
    /// Provides the standard example of overriding equals.
    /// Note that overriding <see cref="Object.Equals(object)"/> means you must override <see cref="Object.GetHashCode"/>.
    /// 
    /// Note also, that if you override <see cref="Object.Equals(object)"/>, you should probably implement <see cref="IEquatable{T}"/> (see <see cref="EquatableClass"/>).
    /// </summary>
    public class OverrideEqualsClass
    {
        public string Value { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            var objAsType = obj as OverrideEqualsClass;

            var isEqual = objAsType.Value.Equals(this.Value);
            return isEqual;
        }

        public override int GetHashCode()
        {
            var hashCode = this.Value.GetHashCode();
            return hashCode;
        }
    }
}
