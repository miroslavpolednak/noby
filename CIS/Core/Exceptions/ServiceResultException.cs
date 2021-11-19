using System;

namespace CIS.Core.Exceptions
{
    public sealed class ServiceResultException : BaseCisException
    {
        public string ExceptionDescription { get; init; } = "";

        public ServiceResultException(int exceptionCode, string? message)
            : base(exceptionCode, message) 
        { }

        public ServiceResultException(int exceptionCode, string message, string exceptionDescription) 
            : base(exceptionCode, message)
        {
            ExceptionDescription = exceptionDescription;
        }
    }
}
