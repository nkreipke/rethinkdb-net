using System;
using System.Runtime.Serialization;

namespace RethinkDb
{
#if !NETSTANDARD
    [Serializable]
#endif
    public abstract class RethinkDbException : Exception
    {
        protected RethinkDbException(string message)
            : base(message)
        {
        }

        protected RethinkDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !NETSTANDARD
        protected RethinkDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}

