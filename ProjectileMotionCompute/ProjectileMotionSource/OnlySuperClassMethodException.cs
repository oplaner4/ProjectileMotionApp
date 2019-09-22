using System;
using System.Runtime.Serialization;

namespace ProjectileMotionSource.Exceptions
{
    [Serializable]
    public class OnlySuperClassMethodException : Exception
    {
        public OnlySuperClassMethodException()
        {}

        public OnlySuperClassMethodException(string message) : base(message)
        {
        }

        public OnlySuperClassMethodException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OnlySuperClassMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}