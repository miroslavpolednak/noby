using System;

namespace CIS.Core.Exceptions
{
    public abstract class BaseCisArgumentException : ArgumentException
    {
        public int ExceptionCode { get; protected set; }

        public BaseCisArgumentException(int exceptionCode, string message, string paramName)
            : base(message, paramName)
        {
            this.ExceptionCode = exceptionCode;
        }
    }
}
