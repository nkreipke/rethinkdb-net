﻿using System;
using System.Runtime.Serialization;

namespace RethinkDb
{
    /// <summary>
    /// Exception thrown when errors occur that appear to be the result of errors or bugs in the RethinkDb
    /// client driver.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class RethinkDbInternalErrorException : RethinkDbException
    {
        internal RethinkDbInternalErrorException(string message)
            : base(message)
        {
        }

        internal RethinkDbInternalErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !NETSTANDARD
         protected RethinkDbInternalErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}

