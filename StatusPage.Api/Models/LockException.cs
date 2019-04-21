using System;
using System.Runtime.Serialization;

namespace StatusPage.Api.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Exception for persistence 
    /// </summary>
    [Serializable]
    public class LockException : Exception
    {
        public LockException() { }

        public LockException(string message) : base(message) { }

        public LockException(Exception innerException) : base("Exception occurred.", innerException) { }

        // Without this constructor, deserialization will fail
        protected LockException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

    }
}