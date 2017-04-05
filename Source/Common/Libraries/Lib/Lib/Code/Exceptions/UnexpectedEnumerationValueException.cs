using System;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace Public.Common.Lib
{
    /// <summary>
    /// An exception specifically for the case in which an enumeration value has not been handled.
    /// </summary>
    /// <typeparam name="T">An enumeration type. Compile-time constraints are not enforced, however a run-time constraint is enforced by the static constructor.</typeparam>
    /// <remarks>
    /// Many enumerations do not grow beyond their initially known values; TRUE, FALSE for example. Some enumerations might have new values added in the future.
    /// For these enumerations, if they are used in a switch statement, there may not be a good default option, or the functionality might be so critical that any user should be made aware of when an enumeration's value is unhandled.
    /// </remarks>
    public class UnexpectedEnumerationValueException<T> : Exception, ISerializable
    // where T : Enum, This constraint does not exist and compile-time workarounds are complicated. Only use this exception with enumerations, else you will receive a run-time exception from the static constructor.
    {
        private const string ValuePropertyName = @"Value";


        #region Static

        // Enforce the enumeration constraint at run-time.
        static UnexpectedEnumerationValueException()
        {
            if (!typeof(T).IsEnum)
            {
                string message = string.Format(@"Generic type parameter T must be an Enumeration. Found: {0}", typeof(T).FullName);
                throw new ArgumentException(message);
            }
        }

        private static string FormatMessage(T value)
        {
            string output = String.Format(@"Unhandled value '{0}' of enumeration {1}.", value, typeof(T).Name);
            return output;
        }

        #endregion

        #region ISerializable Members

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(UnexpectedEnumerationValueException<T>.ValuePropertyName, this.Value);

            base.GetObjectData(info, context);
        }

        #endregion


        public T Value { get; protected set; }


        public UnexpectedEnumerationValueException()
            : base()
        {
        }

        public UnexpectedEnumerationValueException(string message)
            : base(message)
        {
        }

        public UnexpectedEnumerationValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Serialization constructor.
        protected UnexpectedEnumerationValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Value = (T)info.GetValue(UnexpectedEnumerationValueException<T>.ValuePropertyName, typeof(T));
        }

        public UnexpectedEnumerationValueException(T value)
            : this(UnexpectedEnumerationValueException<T>.FormatMessage(value))
        {
        }

        public UnexpectedEnumerationValueException(T value, Exception innerException)
            : this(UnexpectedEnumerationValueException<T>.FormatMessage(value), innerException)
        {
        }
    }
}
