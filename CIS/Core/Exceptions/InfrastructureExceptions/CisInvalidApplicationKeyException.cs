namespace CIS.Core.Exceptions
{
    public sealed class CisInvalidApplicationKeyException : BaseCisArgumentException
    {
        public new const int ExceptionCode = 3;

        public CisInvalidApplicationKeyException(string key) 
            : base(ExceptionCode, $"Application key '{key}' is invalid", nameof(key))
        { }

        public CisInvalidApplicationKeyException(string key, string paramName)
            : base(ExceptionCode, $"Application key '{key}' is invalid", paramName)
        { }
    }
}
