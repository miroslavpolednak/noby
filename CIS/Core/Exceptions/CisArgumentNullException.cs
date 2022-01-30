using System;
using System.Runtime.Serialization;

namespace CIS.Core.Exceptions
{
    [Serializable]
    public sealed class CisArgumentNullException : ArgumentNullException
    {
        public int ExceptionCode { get; init; }

        private CisArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public CisArgumentNullException(int exceptionCode, string message, string paramName)
            : base(message, paramName)
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
