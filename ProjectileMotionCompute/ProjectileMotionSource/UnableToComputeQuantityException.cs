using System;
using System.Runtime.Serialization;

namespace ProjectileMotionSource.Exceptions
{
    [Serializable]
    public class UnableToComputeQuantityException : Exception
    {
        public UnableToComputeQuantityException()
        {}

        public UnableToComputeQuantityException(string message) : base(message)
        {
        }

        public UnableToComputeQuantityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToComputeQuantityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}