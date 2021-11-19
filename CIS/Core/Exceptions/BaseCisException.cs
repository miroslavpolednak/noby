using System;
using System.Runtime.Serialization;

namespace CIS.Core.Exceptions
{
    [Serializable]
    public abstract class BaseCisException : Exception
    {
        public const int UnknownExceptionCode = 0;

        public int ExceptionCode { get; protected set; }

        public BaseCisException(int exceptionCode, string? message)
            : base(message)
        {
            this.ExceptionCode = exceptionCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ExceptionCode), ExceptionCode, typeof(int));
        }
    }
}
