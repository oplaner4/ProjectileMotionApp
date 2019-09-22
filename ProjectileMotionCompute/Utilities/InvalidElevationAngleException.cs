using System;
using System.Runtime.Serialization;

namespace Utilities.Exceptions
{
    [Serializable]
    public class InvalidElevationAngleException : Exception
    {
        public InvalidElevationAngleException()
        {}

        public InvalidElevationAngleException(string message) : base(message)
        {
        }

        public InvalidElevationAngleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidElevationAngleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}