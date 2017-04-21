using System;


namespace Public.Common.Lib.IO.Serialization
{
    /// <summary>
    /// The base class of all serializers.
    /// </summary>
    /// <typeparam name="T">The serialization unit type.</typeparam>
    public abstract class SerializerBase<T> : ISerializer
        where T: class, ISerializationUnit
    {
        #region ISerializer Members

        public virtual void Serialize(ISerializationUnit unit)
        {
            T value = this.Convert(unit);
            this.Serialize(value);
        }

        #endregion


        protected T Convert(ISerializationUnit unit)
        {
            T output;
            if(!this.TryConvert(unit, out output))
            {
                string message = String.Format(@"Wrong type for serialization unit. Required: {0}, found: {1}.", typeof(T).FullName, unit.GetType().FullName);
                throw new ArgumentException(message);
            }

            return output;
        }

        protected bool TryConvert(ISerializationUnit unit, out T value)
        {
            bool output = true;

            value = unit as T;
            if (null == value)
            {
                output = false;
            }

            return output;
        }

        protected abstract void Serialize(T unit);
    }
}
