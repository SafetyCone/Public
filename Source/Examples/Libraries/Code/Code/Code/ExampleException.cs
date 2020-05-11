using System;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace Public.Examples.Code
{
    // Exceptions can be very simple, most of the complications here are due to serialization.
    // See: https://docs.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions
    // See: https://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
    public class ExampleException : Exception, ISerializable
    {
        private const string ValuePropertyName = @"Value";


        #region Static

        private static string FormatMessage(object value)
        {
            string output = String.Format(@"Message goes here. Something about value: {0}.", value);
            return output;
        }

        #endregion

        #region ISerializable

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(ExampleException.ValuePropertyName, this.Value);

            base.GetObjectData(info, context);
        }

        #endregion


        public object Value { get; protected set; }


        // Exceptions generally have many constructors as these are the only methods used.
        public ExampleException()
            : base()
        {
        }

        public ExampleException(string message)
            : base(message)
        {
        }

        public ExampleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Serialization constructor.
        protected ExampleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Value = (object)info.GetValue(ExampleException.ValuePropertyName, typeof(object));
        }

        public ExampleException(object value)
            : this(ExampleException.FormatMessage(value))
        {
            this.Value = value;
        }

        public ExampleException(object value, Exception innerException)
            : this(ExampleException.FormatMessage(value), innerException)
        {
            this.Value = value;
        }
    }
}
