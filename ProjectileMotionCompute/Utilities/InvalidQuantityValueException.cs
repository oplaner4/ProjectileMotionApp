using System;
using System.Runtime.Serialization;

namespace Utilities.Exceptions
{
    [Serializable]
    public class InvalidQuantityValueException : Exception
    {
        public InvalidQuantityValueException()
        {}

        public InvalidQuantityValueException(string message) : base(message)
        {
        }

        public InvalidQuantityValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidQuantityValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}